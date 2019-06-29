import React, {Component} from "react";
import PropTypes from "prop-types";
import {connect} from "react-redux";
import styled from "@emotion/styled";

import History from "./history";
import Payment from "../payment/payment";

import {fetchTransactions} from "../../actions/transactions";
import {getActiveCard, isExpiredCard} from "../../selectors/cards";
import {getTransactionsByDays} from "../../selectors/transactions";
import Authenticated from "../auth/authenticated";
import LoginContract from "./login_contract";
import {logUser, verifyToken} from "../../actions/auth";

const Workspace = styled.div`
  display: flex;
  flex-wrap: wrap;
  max-width: 1200px;
  padding: 15px;
  justify-content: center;
`;

class Home extends Component {
    componentDidMount() {
        this.props.verifyToken();
    }

    render() {
        const {
            transactions,
            activeCard,
            transactionsIsLoading,
            transactionsSkip,
            transactionsCount,
            fetchTransactions,
            isAuth,
            logUser
        } = this.props;

        if (activeCard)
            return (
                <Workspace>
                    <Authenticated isAuth={isAuth}>
                        {isExpiredCard(activeCard.exp) ? (
                            <h1 style={{margin: "15px", fontWeight: "bold"}}>
                                <span role="img">❌</span> Срок действия карты истёк
                            </h1>
                        ) : null}
                        <History
                            transactions={transactions}
                            activeCard={activeCard}
                            isLoading={transactionsIsLoading}
                            skip={transactionsSkip}
                            count={transactionsCount}
                            buttonClick={fetchTransactions}
                        />
                        <Payment/>
                    </Authenticated>
                </Workspace>
            );
        else return (<Workspace>
            {!isAuth && <LoginContract logUser={logUser}/>}
        </Workspace>);
    }
}

Home.propTypes = {
    transactions: PropTypes.arrayOf(PropTypes.object),
    activeCard: PropTypes.object,
    transactionsIsLoading: PropTypes.bool.isRequired,
    transactionsSkip: PropTypes.number.isRequired
};

const mapStateToProps = state => ({
    isAuth: state.auth.isAuth,
    transactions: getTransactionsByDays(state),
    activeCard: getActiveCard(state),
    transactionsIsLoading: state.transactions.isLoading,
    transactionsSkip: state.transactions.skip,
    transactionsCount: state.transactions.count
});

const mapDispatchToProps = dispatch => ({
    verifyToken: () => dispatch(verifyToken()),
    logUser: (userName, password) => dispatch(logUser(userName, password)),
    fetchTransactions: (number, skip) => dispatch(fetchTransactions(number, skip))
});

export default connect(mapStateToProps, mapDispatchToProps)(Home);
