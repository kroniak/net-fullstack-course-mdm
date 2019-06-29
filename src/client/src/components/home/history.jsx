import React from "react";
import styled from "@emotion/styled";
import moment from "moment";

import Island from "../misc/island";

const HistoryLayout = styled(Island)`
  width: 800px;
  max-height: 622px;
  overflow-y: scroll;
  padding: 0;
  background-color: rgba(0, 0, 0, 0.05);
  display: flex;
  flex-direction: column;
`;

const HistoryTitle = styled.div`
  padding-left: 12px;
  color: rgba(0, 0, 0, 0.4);
  font-size: 15px;
  line-height: 30px;
  text-transform: uppercase;
`;

const HistoryButtonBar = styled.div`
  padding-left: 12px;
  color: rgba(0, 0, 0, 0.4);
  font-size: 15px;
  line-height: 30px;
  text-transform: uppercase;
  display: grid;
  grid-template-columns: 1fr 1fr;
`;

const HistoryItem = styled.div`
  display: flex;
  justify-content: space-around;
  align-items: center;
  height: 74px;
  min-height: 74px;
  font-size: 15px;
  font-weight: bold;
  white-space: nowrap;

  &:nth-child(even) {
    background-color: #fff;
  }

  &:nth-child(odd) {
    background-color: rgba(255, 255, 255, 0.72);
  }

  color: ${({isInvalid}) => (isInvalid ? "#F44336" : "#000")};
`;

const HistoryItemIcon = styled.div`
  width: 50px;
  height: 50px;
  border-radius: 25px;
  background-color: #159761;
  background-image: url(${({bankSmLogoUrl}) => bankSmLogoUrl});
  background-size: contain;
  background-repeat: no-repeat;
`;

const HistoryItemTitle = styled.div`
  width: 360px;
  overflow: hidden;
  text-overflow: ellipsis;
`;

const HistoryItemTime = styled.div`
  width: 50px;
`;

const HistoryItemSum = styled.div`
  width: 72px;
  overflow: hidden;
  text-overflow: ellipsis;
  color: ${({color}) => color};
`;

const HistoryButton = styled.span`
  align-self: center;
  justify-self: center;
  cursor: pointer;
  color: ${({isActive}) => (isActive ? "#black" : "rgba(0, 0, 0, 0.05)")};
`;

const History = ({
                     transactions,
                     activeCard,
                     isLoading,
                     skip,
                     count,
                     buttonClick
                 }) => {
    const renderCardsHistory = () => {
        if (isLoading) return <HistoryItem>Загрузка...</HistoryItem>;

        const result = [];
        const today = moment().format("L");

        if (transactions.length === 0)
            result.push(
                <HistoryItem key={today + "HistoryItem"}>
                    Операций не найдено
                </HistoryItem>
            );
        else {
            transactions.forEach(item => {
                if (item.key === today)
                    result.push(<HistoryTitle key={item.key}>Сегодня</HistoryTitle>);
                else {
                    if (result.length === 0) {
                        result.push(
                            <HistoryTitle key={today + "HistoryTitle"}>Сегодня</HistoryTitle>
                        );
                        result.push(
                            <HistoryItem key={today + "HistoryItem"}>
                                Операций за этот день нет
                            </HistoryItem>
                        );
                    }

                    result.push(<HistoryTitle key={item.key}>{item.key}</HistoryTitle>);
                }

                result.push(renderCardsDay(item.data));
            });
        }

        return result;
    };

    const renderCardsDay = arr =>
        arr.map(item => {
            return (
                <HistoryItem key={item.id}>
                    <HistoryItemIcon bankSmLogoUrl={activeCard.theme.bankSmLogoUrl}/>
                    <HistoryItemTitle>{item.title}</HistoryItemTitle>
                    <HistoryItemTime>{item.hhmm}</HistoryItemTime>
                    <HistoryItemSum color={item.debit > 0 ? "green" : "red"}>
                        {item.debit > 0 ? "+" : "-"}
                        {`${Number(item.sum.toFixed(2))} ${activeCard.currencySign}`}
                    </HistoryItemSum>
                </HistoryItem>
            );
        });

    const renderSkipButton = (skip, count) => {
        const renderButton = (nextSkip, link, isActive) => (
            <HistoryButton
                onClick={
                    isActive ? () => buttonClick(activeCard.number, nextSkip) : null
                }
                isActive={isActive}
            >
                {link}
            </HistoryButton>
        );

        let isActive = {prev: false, next: false};

        if (count === 10) isActive.next = true;
        if (skip >= 10) isActive.prev = true;

        return (
            <HistoryButtonBar>
                {renderButton(skip - 10, "<<<<<<", isActive.prev)}
                {renderButton(skip + 10, ">>>>>>", isActive.next)}
            </HistoryButtonBar>
        );
    };

    return (
        <HistoryLayout>
            {renderSkipButton(skip, count)}
            {renderCardsHistory()}
        </HistoryLayout>
    );
};

export default History;
