import CardInfo from "card-info";
import moment from "moment";
import {createSelector} from "reselect";

/**
 * Returns currency sign
 * @param {String} currency
 * @returns {String}
 */
export const getSignByCurrency = currency => {
    switch (currency) {
        case 0:
            return "₽";
        case 1:
            return "$";
        case 2:
            return "€";
        default:
            return "₽";
    }
};

/**
 * Returns currency
 * @param {String} sign
 * @returns {number}
 */
export const getCurrencyBySign = sign => {
    switch (sign) {
        case "₽":
            return 0;
        case "$":
            return 1;
        case "€":
            return 2;
        default:
            return 0;
    }
};

export const getTypeByName = name => {
    switch (name) {
        case "MASTERCARD":
            return 1;
        case "VISA":
            return 2;
        case "MAESTRO":
            return 3;
        case "MIR":
            return 3;
        default:
            return 0;
    }
};

/**
 * Подготавливает данные карты
 *
 * @param {Object} card данные карты
 * @returns {Object[]}
 */
const prepareCardData = ({number, balance = 0, currency, exp}) => {
    const {
        numberNice,
        bankName,
        backgroundColor,
        textColor,
        bankLogoSvg,
        brandLogoSvg,
        bankAlias
    } = new CardInfo(number, {
        banksLogosPath: "/assets/",
        brandsLogosPath: "/assets/"
    });

    return {
        number,
        balance,
        currency,
        currencySign: getSignByCurrency(currency),
        numberNice,
        exp,
        bankName,
        theme: {
            bgColor: backgroundColor,
            textColor,
            bankLogoUrl: bankLogoSvg,
            brandLogoUrl: brandLogoSvg,
            bankSmLogoUrl: `/assets/${bankAlias}-history.svg`
        }
    };
};

const getCards = state => state.cards.data;

/**
 * Подготавливает данные карт
 *
 * @returns {Object[]}
 * @param cards
 */
const prepareCardsData = cards => cards.map(card => prepareCardData(card));
const getActiveCardNumber = state => state.cards.activeCardNumber;

export const getPreparedCards = createSelector([getCards], cards =>
    prepareCardsData(cards)
);

export const getActiveCard = createSelector(
    [getActiveCardNumber, getPreparedCards],
    (activeCardNumber, cards) =>
        cards.find(card => card.number === activeCardNumber)
);

export const getFilteredCards = createSelector(
    [getActiveCardNumber, getPreparedCards],
    (activeCardNumber, cards) =>
        cards.filter(card => card.number !== activeCardNumber)
);

export const isExpiredCard = exp => {
    if (exp) {
        const currentYear = moment();
        const cardYear = moment(exp, "MM/YY");
        return cardYear <= currentYear;
    } else {
        return false;
    }
};
