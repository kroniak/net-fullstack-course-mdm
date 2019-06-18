import React, {Component} from "react";
import PropTypes from "prop-types";
import {connect} from "react-redux";
import styled from "@emotion/styled";

import History from "./history";
import Payment from "../payment/payment";
import {isExpiredCard} from "../../selectors/cards";

const Workspace = styled.div`
  display: flex;
  flex-wrap: wrap;
  max-width: 1200px;
  padding: 15px;
  justify-content: center;
`;

class Home extends Component {
    componentDidMount() {
        this.props.fetchCards();
    }

    render() {
        const {
            transactions,
            activeCard,
            transactionsIsLoading,
            transactionsSkip,
            transactionsCount,
        } = this.props;

        if (activeCard)
            return (
                <Workspace>
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
                    />
                    <Payment/>
                </Workspace>
            );
        else return <Workspace/>;
    }
}

Home.propTypes = {
    transactions: PropTypes.arrayOf(PropTypes.object),
    activeCard: PropTypes.object,
    transactionsIsLoading: PropTypes.bool.isRequired,
    transactionsSkip: PropTypes.number.isRequired
};

const mapStateToProps = state => ({
    transactions: [],
    activeCard: 0,
    transactionsIsLoading: false,
    transactionsSkip: 0,
    transactionsCount: 0
});

const mapDispatchToProps = dispatch => ({
    fetchCards: () => dispatch({type: "TODO"}),
});

export default connect(mapStateToProps, mapDispatchToProps)(Home);
