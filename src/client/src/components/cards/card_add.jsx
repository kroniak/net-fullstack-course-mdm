import React, {Component} from "react";
import styled from "@emotion/styled";
import Card from "./card";
import Button from "../misc/button.jsx";
import CardInfo from "card-info";
import {getCurrencyBySign, getTypeByName} from "../../selectors/cards";
import PropTypes from "prop-types";

const CardAddLayout = styled.div`
  flex: 1;
  width: 260px;
`;

const Title = styled.div`
  font-size: 20px;
  font-weight: 500;
  letter-spacing: 0.9px;
  color: #ffffff;
  margin-bottom: 10px;
`;

const Footer = styled.div`
  display: flex;
  justify-content: space-between;
  margin-top: 35px;
`;

class CardAdd extends Component {
    constructor(props) {
        super(props);

        this.state = {
            name: "Моя карта",
            cardType: "VISA",
            currencySign: "₽"
        };
    };

    onAddClick = () => {
        const {name, cardType, currencySign} = this.state;

        if (name && cardType && currencySign)
            this.props.addCard(
                getCurrencyBySign(currencySign),
                getTypeByName(cardType),
                name
            );
    };

    onCardNameChange = e => {
        this.setState({
            name: e.target.value
        });
    };

    onCardCurrencyChange = currencySign => {
        this.setState({
            currencySign
        });
    };

    onCardTypeChange = cardType => {
        this.setState({
            cardType
        });
    };

    getDisplayValues = (currencySign, cardType, name) => {
        const {
            backgroundColor,
            textColor,
            bankLogoSvg,
            bankAlias
        } = new CardInfo("5101264263612037", {
            banksLogosPath: "/assets/",
            brandsLogosPath: "/assets/"
        });

        return {
            name,
            currencySign,
            cardType,
            theme: {
                bgColor: backgroundColor,
                textColor: textColor,
                bankLogoUrl: bankLogoSvg,
                bankSmLogoUrl: `/assets/${bankAlias}-history.svg`
            }
        };
    };

    render() {
        const {onCancelClick} = this.props;
        const {name, cardType, currencySign} = this.state;

        return (
            <CardAddLayout>
                <Title> Открыть
                    новую
                    карту
                </Title>
                <Card
                    type="form"
                    data={this.getDisplayValues(currencySign, cardType, name)}
                    onCardNameChange={this.onCardNameChange}
                    onCardCurrencyChange={this.onCardCurrencyChange}
                    onCardTypeChange={this.onCardTypeChange}
                />
                <Footer>
                    <div
                        onClick={this.onAddClick}>
                        <Button
                            bgColor="#d3292a"
                            textColor="#fff">
                            Открыть
                        </Button>
                    </div>
                    <div
                        onClick={() => onCancelClick(true)}>
                        <Button
                            bgColor="#1F1F1F"
                            textColor="#fff">
                            Отмена
                        </Button>
                    </div>
                </Footer>
            </CardAddLayout>);
    };
}

CardAdd.propTypes = {
    onCancelClick: PropTypes.func.isRequired,
    addCard: PropTypes.func.isRequired
};

export default CardAdd;
