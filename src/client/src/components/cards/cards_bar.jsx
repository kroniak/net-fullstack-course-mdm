import React, {Component} from "react";
import {connect} from "react-redux";
import styled from "@emotion/styled";

import {addCard, changeActiveCard} from "../../actions/cards";
import {getPreparedCards} from "../../selectors/cards";

import Card from "./card";
import CardAdd from "./card_add";
import PropTypes from "prop-types";
import Authenticated from "../auth/authenticated";

const Layout = styled.div`
  display: flex;
  flex-direction: column;
  position: relative;
  padding: 20px;
`;

const CardsList = styled.div`
  flex: 1;
  font-size: 15px;
`;

const Footer = styled.footer`
  color: rgba(255, 255, 255, 0.2);
  font-size: 15px;
`;

class CardsBar extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isCardAdding: false
        };
    }

    onSetAddMode = event => {
        event.stopPropagation();
        this.setState({
            isCardAdding: true
        });
    };

    onCancelMode = () => {
        this.setState({
            isCardAdding: false
        });
    };

    onAddClickWrapper = (currency, type, name) => {
        this.setState({
            isCardAdding: false
        });
        this.props.onAddClick(currency, type, name);
    };

    renderCards = () => {
        const {isLoading, cards, activeCardNumber, onClick} = this.props;

        return isLoading ? (
            <div/>
        ) : (
            cards.map(card => (
                <Card
                    key={card.number}
                    data={card}
                    active={card.number === activeCardNumber}
                    onClick={() => onClick(card.number)}
                />
            ))
        );
    };

    render() {
        const {isCardAdding, isLoading} = this.state;

        if (isCardAdding)
            return (
                <Layout>
                    <CardAdd
                        onCancelClick={() => this.onCancelMode()}
                        addCard={(currency, type, name) =>
                            this.onAddClickWrapper(currency, type, name)
                        }
                    />
                    <Footer>Альфа-Банк</Footer>
                </Layout>
            );

        return (
            <Layout>
                <Authenticated isAuth={this.props.isAuth}>
                    <CardsList>
                        {this.renderCards()}
                        {isLoading ? (
                            <div/>
                        ) : (
                            <Card type="new" onChangeAddMode={e => this.onSetAddMode(e)}/>
                        )}
                    </CardsList>
                </Authenticated>
                <Footer>Альфа-Банк</Footer>
            </Layout>
        );
    };
}

CardsBar.propTypes = {
    cards: PropTypes.array.isRequired,
    activeCardNumber: PropTypes.string,
    error: PropTypes.string,
    onClick: PropTypes.func.isRequired,
    isLoading: PropTypes.bool.isRequired
};

const mapStateToProps = state => ({
    isAuth: state.auth.isAuth,
    cards: getPreparedCards(state),
    error: state.cards.error,
    activeCardNumber: state.cards.activeCardNumber,
    isLoading: state.cards.isLoading
});

const mapDispatchToProps = dispatch => ({
    onClick: number => dispatch(changeActiveCard(number)),
    onAddClick: (currency, type, name) => dispatch(addCard(currency, type, name))
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(CardsBar);
