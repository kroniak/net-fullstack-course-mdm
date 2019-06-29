import React, {Component} from "react";
import styled from "@emotion/styled";
import Island from "../misc/island";
import Title from "../misc/title";
import Button from "../misc/button";
import Input from "../misc/input";

const WithdrawTitle = styled(Title)`
  text-align: center;
`;

const WithdrawLayout = styled(Island)`
  margin: 0;
  width: 440px;
  display: flex;
  flex-direction: column;
  align-items: center;
  box-sizing: border-box;
`;

const InputField = styled.div`
  margin: 20px 0;
  position: relative;
`;

const FormInput = styled(Input)`
  max-width: 200px;
  padding-right: 20px;
  background-color: rgba(0, 0, 0, 0.08);
  color: #000;
`;


/**
 * Класс компонента Withdraw
 */
class LoginContract extends Component {
    /**
     * Конструктор
     * @param {Object} props свойства компонента Withdraw
     */
    constructor(props) {
        super(props);

        this.state = {
            userField: "alice@alfabank.ru",
            passwordField: "12345678"
        };
    }

    /**
     * Обработка изменения значения в input
     * @param {Event} event событие изменения значения input
     */
    onChangeUserInputValue = event => {
        if (event) event.preventDefault();

        this.setState({
            userField: event.target.value
        });
    };

    /**
     * Обработка изменения значения в input
     * @param {Event} event событие изменения значения input
     */
    onChangePasswordInputValue = event => {
        if (event) event.preventDefault();

        this.setState({
            passwordField: event.target.value
        });
    };

    /**
     * Отправка формы
     * @param {Event} event событие отправки формы
     */
    onSubmitForm = event => {
        if (event) event.preventDefault();

        const {userField, passwordField} = this.state;
        if (userField && passwordField) {
            this.props.logUser(userField, passwordField);

            this.setState({
                    passwordField: ""
                }
            );
        }
    };

    /**
     * Функция отрисовки компонента
     * @returns {JSX}
     */
    render() {
        const {userField, passwordField} = this.state;
        return (
            <WithdrawLayout>
                <form onSubmit={event => this.onSubmitForm(event)}>
                    <WithdrawTitle>Пожалуйста войдите</WithdrawTitle>
                    <InputField>
                        <FormInput
                            name="UserName"
                            value={userField}
                            onChange={event => this.onChangeUserInputValue(event)}
                        />
                    </InputField>
                    <InputField>
                        <FormInput
                            type="Password"
                            name="Password"
                            value={passwordField}
                            onChange={event => this.onChangePasswordInputValue(event)}
                        />
                    </InputField>
                    <Button type="submit">Войти</Button>
                </form>
            </WithdrawLayout>
        );
    }
}

export default LoginContract;
