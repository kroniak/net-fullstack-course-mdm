import React from "react";
import PropTypes from "prop-types";
import {connect} from "react-redux";

import PaymentContract from "./payment_contract";
import {PaymentError, PaymentSuccess} from "./payment_screens";

import {getActiveCard, getFilteredCards, isExpiredCard} from "../../selectors/cards";

import {repeatTransferMoney, TransferMoney} from "../../actions/payments";

const Payment = props => {
    const {
        paymentState,
        onRepeatPaymentClick,
        onPaymentSubmit,
        activeCard,
        inactiveCardsList
    } = props;

    if (isExpiredCard(activeCard.exp)) return null;
    if (paymentState.stage === "success")
        return (
            <PaymentSuccess
                activeCard={activeCard}
                transaction={paymentState.transaction}
                repeatPayment={() => onRepeatPaymentClick()}
            />
        );
    else if (paymentState.stage === "contract" && inactiveCardsList.length > 0)
        return (
            <PaymentContract
                activeCard={activeCard}
                inactiveCardsList={inactiveCardsList}
                onPaymentSubmit={(from, to, sum) => onPaymentSubmit(from, to, sum)}
            />
        );
    else
        return (
            <PaymentError
                activeCard={activeCard}
                transaction={paymentState.transaction}
                error={paymentState.error}
                repeatPayment={() => onRepeatPaymentClick()}
            />
        );
};

Payment.propTypes = {
    paymentState: PropTypes.object,
    activeCard: PropTypes.object,
    currencyState: PropTypes.object,
    inactiveCardsList: PropTypes.arrayOf(PropTypes.object),
    onPaymentSubmit: PropTypes.func.isRequired,
    onRepeatPaymentClick: PropTypes.func.isRequired
};

const mapStateToProps = state => ({
    paymentState: state.payment,
    activeCard: getActiveCard(state),
    inactiveCardsList: getFilteredCards(state)
});

const mapDispatchToProps = dispatch => ({
    /**
     * Обработка успешного платежа
     */
    onPaymentSubmit: (from, to, sum) => dispatch(TransferMoney(from, to, sum)),

    /**
     * Повторить платеж
     */
    onRepeatPaymentClick: () => dispatch(repeatTransferMoney())
});

export default connect(mapStateToProps, mapDispatchToProps)(Payment);
