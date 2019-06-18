import React from "react";
import styled from "@emotion/styled";
import PropTypes from "prop-types";

import Island from "../misc/island";
import Title from "../misc/title";

const WithdrawLayout = styled(Island)`
  width: 350px;
  display: flex;
  flex-direction: column;
  background-color: #353536;
  position: relative;
  color: #fff;
  box-sizing: border-box;
`;

const WithdrawLayoutError = styled(Island)`
  width: 350px;
  display: flex;
  flex-direction: column;
  background-color: #7f0d00;
  position: relative;
  color: #fff;
  box-sizing: border-box;
`;

const CheckIcon = styled.div`
  width: 48px;
  height: 48px;
  background-image: url(/assets/round-check.svg);
  position: absolute;
  top: 14px;
  right: 20px;
`;

const ErrorIcon = styled.div`
  width: 48px;
  height: 48px;
  background-image: url(/assets/round-error.svg);
  background-size: 48px;
  position: absolute;
  top: 14px;
  right: 20px;
  -webkit-filter: invert(100%);
  filter: invert(100%);
`;

const Header = styled(Title)`
  color: #fff;
`;

const SectionGroup = styled.div`
  margin-bottom: 20px;
`;

const Section = styled.div`
  margin-bottom: 20px;
  width: 100%;
`;

const SectionLabel = styled.div`
  font-size: 13px;
  text-align: left;
`;

const SectionValue = styled.div`
  font-size: 13px;
  letter-spacing: 0.6px;
`;

const RepeatPayment = styled.button`
  font-size: 13px;
  background-color: rgba(0, 0, 0, 0.08);
  height: 42px;
  display: flex;
  justify-content: center;
  align-items: center;
  border: none;
  width: 100%;
  position: absolute;
  left: 0;
  bottom: 0;
  cursor: pointer;
  text-transform: uppercase;
`;

export const PaymentSuccess = ({activeCard, transaction, repeatPayment}) => {
    const {sum, to} = transaction;

    return (
        <WithdrawLayout>
            <CheckIcon/>
            <SectionGroup>
                <Header>Перевод на карту выполнен</Header>
                <Section>
                    <SectionLabel>Название платежа:</SectionLabel>
                    <SectionValue>Перевод на привязанную карту</SectionValue>
                </Section>
                <Section>
                    <SectionLabel>Карта на которую переводили:</SectionLabel>
                    <SectionValue>{to}</SectionValue>
                </Section>
                <Section>
                    <SectionLabel>Сумма:</SectionLabel>
                    <SectionValue>
                        {sum} {activeCard.currencySign}
                    </SectionValue>
                </Section>
            </SectionGroup>
            <RepeatPayment onClick={repeatPayment}>
                Отправить еще один перевод
            </RepeatPayment>
        </WithdrawLayout>
    );
};

PaymentSuccess.propTypes = {
    activeCard: PropTypes.object,
    transaction: PropTypes.shape({
        sum: PropTypes.string,
        number: PropTypes.string
    }).isRequired,
    repeatPayment: PropTypes.func.isRequired
};

export const PaymentError = ({
                                 activeCard,
                                 transaction,
                                 repeatPayment,
                                 error
                             }) => {
    const {sum, to} = transaction;

    return (
        <WithdrawLayoutError>
            <ErrorIcon/>
            <SectionGroup>
                <Header>Ошибка</Header>
                <Section>
                    <SectionLabel>Название платежа:</SectionLabel>
                    <SectionValue>Перевод на привязанную карту</SectionValue>
                </Section>
                <Section>
                    <SectionLabel>Карта на которую переводили:</SectionLabel>
                    <SectionValue>{to}</SectionValue>
                </Section>
                <Section>
                    <SectionLabel>Сумма:</SectionLabel>
                    <SectionValue>
                        {sum} {activeCard.currencySign}
                    </SectionValue>
                </Section>
                <Section>
                    <SectionLabel>Ошибка:</SectionLabel>
                    <SectionValue>{error}</SectionValue>
                </Section>
            </SectionGroup>
            <RepeatPayment onClick={repeatPayment}>
                Отправить еще один перевод
            </RepeatPayment>
        </WithdrawLayoutError>
    );
};

PaymentError.propTypes = {
    activeCard: PropTypes.object,
    transaction: PropTypes.shape({
        sum: PropTypes.string,
        number: PropTypes.string
    }).isRequired,
    repeatPayment: PropTypes.func.isRequired,
    error: PropTypes.string.isRequired
};
