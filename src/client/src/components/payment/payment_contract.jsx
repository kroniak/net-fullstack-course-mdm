import React, {Component} from "react";
import PropTypes from "prop-types";
import styled from "@emotion/styled";

import Card from "../cards/card";
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

const SumInput = styled(Input)`
  max-width: 200px;
  padding-right: 20px;
  background-color: rgba(0, 0, 0, 0.08);
  color: #000;
`;

const Currency = styled.span`
  font-size: 12px;
  position: absolute;
  right: 10px;
  top: 8px;
`;

/**
 * Класс компонента Withdraw
 */
class PaymentContract extends Component {
    /**
     * Конструктор
     * @param {Object} props свойства компонента Withdraw
     */
    constructor(props) {
        super(props);

        this.state = {
            withdrawCardIndex: 0,
            sum: 0
        };
    }

    /**
     * Обработчик переключения карты
     *
     * @param {Number} withdrawCardIndex индекс выбранной карты
     */
    onCardChange = withdrawCardIndex => {
        this.setState({
            withdrawCardIndex
        });
    }

    /**
     * Обработка изменения значения в input
     * @param {Event} event событие изменения значения input
     */
    onChangeInputValue = event => {
        if (event) event.preventDefault();

        this.setState({
            sum: event.target.value
        });
    }

    /**
     * Отправка формы
     * @param {Event} event событие отправки формы
     */
    onSubmitForm = event => {
        if (event) event.preventDefault();

        const {activeCard, inactiveCardsList, onPaymentSubmit} = this.props;
        const {sum, withdrawCardIndex} = this.state;

        const isNumberSum = !isNaN(parseFloat(sum)) && isFinite(sum);

        if (!isNumberSum || sum <= 0) return;

        const withdrawCard = inactiveCardsList[withdrawCardIndex];

        onPaymentSubmit(activeCard.number, withdrawCard.number, sum);

        this.setState({sum: 0});
    }

    /**
     * Функция отрисовки компонента
     * @returns {JSX}
     */
    render() {
        const {inactiveCardsList, activeCard} = this.props;

        if (inactiveCardsList.length === 0) return <div/>;

        const {sum, withdrawCardIndex} = this.state;
        return (
            <WithdrawLayout>
                <form onSubmit={event => this.onSubmitForm(event)}>
                    <WithdrawTitle>Перевести деньги на карту</WithdrawTitle>
                    <Card
                        type="select"
                        data={inactiveCardsList}
                        activeCardIndex={withdrawCardIndex}
                        onCardChange={id => this.onCardChange(id)}
                    />
                    <InputField>
                        <SumInput
                            name="sum"
                            value={sum}
                            onChange={event => this.onChangeInputValue(event)}
                        />
                        <Currency>{activeCard.currencySign}</Currency>
                    </InputField>
                    <Button type="submit">Перевести</Button>
                </form>
            </WithdrawLayout>
        );
    }
}

PaymentContract.propTypes = {
    activeCard: PropTypes.object,
    inactiveCardsList: PropTypes.arrayOf(PropTypes.object),
    onPaymentSubmit: PropTypes.func.isRequired
};

export default PaymentContract;
