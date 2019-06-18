import React from "react";
import PropTypes from "prop-types";
import styled from "@emotion/styled";
import Select from "./select";
import {isExpiredCard} from "../../selectors/cards";

const CardLayout = styled.div`
  position: relative;
  width: 270px;
  height: 164px;
  box-sizing: border-box;
  margin-bottom: ${({isSingle}) => (isSingle ? 0 : "15px")};
  padding: ${({isExpired}) =>
    !isExpired ? "25px 20px 20px 25px" : "25px 18px 18px 25px"};
  border-radius: 4px;
  background-color: ${({bgColor, active}) =>
    active ? bgColor : "rgba(255, 255, 255, 0.1)"};
  cursor: ${({cursor}) => (cursor ? cursor : "pointer")};
  border: ${({isExpired}) => (!isExpired ? "0" : "thin solid red")};
`;

const CardLogo = styled.div`
  height: 28px;
  margin-bottom: ${({isTight}) => (isTight ? "20px" : "25px")};
  background-image: url(${({url}) => url});
  background-size: contain;
  background-repeat: no-repeat;
  filter: ${({active}) => (active ? "none" : "grayscale(100%) opacity(60%)")};
`;

const CardNumber = styled.div`
  margin-bottom: 20px;
  color: ${({active, textColor}) =>
    active ? textColor : "rgba(255, 255, 255, 0.6)"};
  font-size: 16px;
  font-family: "OCR A Std Regular";
`;

const CardNumberInput = styled.input`
  margin-bottom: ${({isTight}) => (isTight ? "10px" : "20px")};
  color: ${({active, textColor}) =>
    active ? textColor : "rgba(255, 255, 255, 0.6)"};
  font-size: 16px;
  font-family: "OCR A Std Regular";
  background: transparent;
  border: none;
`;

const CardType = styled.div`
  height: 26px;
  background-image: url(${({url}) => url});
  background-size: contain;
  background-repeat: no-repeat;
  background-position-x: right;
  opacity: ${({active}) => (active ? "1" : "0.6")};

  padding-right: 60px;
  padding-top: 4px;

  color: ${({active, textColor}) =>
    active ? textColor : "rgba(255, 255, 255, 0.6)"};
  font-size: 16px;
  font-family: "OCR A Std Regular";
`;

const CardCurrency = styled.div`
  display: inline-block;
  width: 105px;
  font-weight: bold;
  font-size: 16px;
  font-family: "OCR A Std Regular";
  text-align: left !important;
  color: ${({color}) => (color ? color : "inherit")};
`;

const NewCardLayout = styled(CardLayout)`
  background-color: transparent;
  background-image: url("/assets/cards-add.svg");
  background-repeat: no-repeat;
  background-position: center;
  box-sizing: border-box;
  border: 2px dashed rgba(255, 255, 255, 0.2);
`;

const CardSelect = styled(Select)`
  width: 100%;
  margin-bottom: 15px;
`;

const CurrencySelect = styled(Select)`
  width: 60px;
  float: left;
  margin-right: 30px;
`;

const TypeSelect = styled(CurrencySelect)`
  width: 108px;
  margin-right: 27px;
`;

const Card = props => {
    const {type} = props;

    if (type === "new")
        return <NewCardLayout onClick={props.onChangeAddMode}/>;

    const {data, isSingle} = props;

    if (type === "select") {
        const {activeCardIndex} = props;
        const selectedCard = data[activeCardIndex];
        const {bgColor, bankLogoUrl, brandLogoUrl} = selectedCard.theme;

        return (
            <CardLayout
                active={true}
                bgColor={bgColor}
                isSingle={isSingle}
                isExpired={isExpiredCard(data.exp)}>
                <CardLogo url={bankLogoUrl} active={true}/>
                <CardSelect
                    value={selectedCard.number}
                    onChange={id => props.onCardChange(id)}>
                    {data.map((card, index) => (
                        <Select.Option key={index} value={`${index}`}>
                            {card.number}
                        </Select.Option>
                    ))}
                </CardSelect>
                <CardType url={brandLogoUrl} active={true}>
                    <CardCurrency color={"#fff"}>
                        {selectedCard.currencySign}
                    </CardCurrency>
                </CardType>
            </CardLayout>
        );
    }

    if (type === "form") {
        const {name, cardType, currencySign, theme} = data;
        const {bgColor, textColor, bankLogoUrl} = theme;
        const {
            onCardNameChange,
            onCardCurrencyChange,
            onCardTypeChange
        } = props;

        return (
            <CardLayout
                active={true}
                bgColor={bgColor}
                isCardsEditable={false}
                isSingle={true}
                cursor={"auto"}
                isExpired={isExpiredCard(data.exp)}
            >
                <CardLogo url={bankLogoUrl} active={true} isTight={true}/>
                <CardNumberInput
                    textColor={textColor}
                    active={true}
                    value={name}
                    onChange={onCardNameChange}
                    isTight={true}
                />
                <CurrencySelect
                    value={currencySign}
                    onChange={sign => onCardCurrencyChange(sign)}
                >
                    {["₽", "$", "€"].map((sign, index) => (
                        <Select.Option key={index} value={sign}>
                            {sign}
                        </Select.Option>
                    ))}
                </CurrencySelect>
                <TypeSelect
                    value={cardType}
                    onChange={type => onCardTypeChange(type)}
                >
                    {["MASTERCARD", "VISA", "MAESTRO"].map((type, index) => (
                        <Select.Option key={index} value={type}>
                            {type}
                        </Select.Option>
                    ))}
                </TypeSelect>
            </CardLayout>
        );
    }

    const {active, onClick} = props;
    const {numberNice, theme, exp, currencySign} = data;

    const {bgColor, textColor, bankLogoUrl, brandLogoUrl} = theme;
    const isExpired = isExpiredCard(data.exp);
    const themedBrandLogoUrl = active
        ? brandLogoUrl
        : brandLogoUrl.replace(/-colored.svg$/, "-white.svg");

    return (
        <CardLayout
            active={active}
            bgColor={bgColor}
            onClick={onClick}
            cursor={isExpired ? "not-allowed" : "pointer"}
            isSingle={isSingle}
            isExpired={isExpired}
        >
            <CardLogo url={bankLogoUrl} active={active}/>
            <CardNumber textColor={textColor} active={active}>
                {numberNice}
            </CardNumber>
            <CardType
                url={themedBrandLogoUrl}
                active={active}
                textColor={textColor}
            >
                <CardCurrency>{currencySign}</CardCurrency>
                <span>{exp}</span>
            </CardType>
        </CardLayout>
    );
};

Card.propTypes = {
    data: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
    type: PropTypes.string,
    active: PropTypes.bool,
    onClick: PropTypes.func,
    isSingle: PropTypes.bool,
    onChangeAddMode: PropTypes.func,
    onCardChange: PropTypes.func
};

export default Card;