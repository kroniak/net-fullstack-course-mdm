import React from "react";
import {connect} from "react-redux";
import styled from "@emotion/styled";
import Title from "../misc/title";
import UserInfo from "./user-info";

import {getActiveCard} from "../../selectors/cards";

const HeaderLayout = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 74px;
  background: #fff;
  padding: 20px 30px;
  box-sizing: border-box;
  border-bottom: 1px solid rgba(0, 0, 0, 0.06);
`;

const BalanceSum = styled.span`
  font-weight: bold;
`;

class Header extends React.Component {
    renderBalance = () => {
        const {activeCard} = this.props;

        if (activeCard) {
            let balance = activeCard.balance || 0;
            return (
                <Title style={{margin: 0}}>
                    Баланс:
                    <BalanceSum>{` ${Number(balance.toFixed(2))} ${
                        activeCard.currencySign
                        }`}
                    </BalanceSum>
                </Title>
            );
        }
    }

    render() {
        return (
            <HeaderLayout>
                {this.renderBalance()}
                <Title>Электронный кошелек</Title>
                <UserInfo/>
            </HeaderLayout>
        );
    }
}

const mapStateToProps = state => ({
    activeCard: getActiveCard(state)
});

export default connect(mapStateToProps)(Header);
