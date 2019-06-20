import moment from "moment";
import { createSelector } from "reselect";
import { getActiveCard } from "./cards";

const separateByDates = (transactions, activeCard) => {
  const creditDebitFlag = ({ from, to }) => {
    if (from == null) return 1;
    else if (to == null) return -1;
    else if (activeCard.number === from) return 0;
    else if (activeCard.number === to) return 2;
  };

  const getHistoryItemTitle = item => {
    let flag = creditDebitFlag(item);

    if (flag === 1) return "Операция пополнения при открытии";
    else if (flag === 0) return `Операция перевода на ${item.to}`;
    else if (flag === 2) return `Операция перевода с ${item.from}`;
    else if (flag === -1) return `Операция снятия комиссии с ${item.from}`;
  };

  const sortedData = transactions.sort((a, b) => {
    if (a.dateTime <b.dateTime) return 1;
    else if (a.dateTime > b.dateTime) return -1;
    else return 0;
  });

  return sortedData.reduce((result, item) => {
    const { from, to, dateTime, sum } = item;

    const key = moment(dateTime).format("L");

    let row = result.find(item => item.key === key);

    if (!row) {
      row = {
        key,
        data: []
      };
      result.push(row);
    }

    row.data.push({
      id: dateTime + from,
      hhmm: moment(dateTime).format("HH:mm"),
      title: getHistoryItemTitle({
        from,
        to
      }),
      sum,
      debit: creditDebitFlag({
        from,
        to
      })
    });

    return result;
  }, []);
};

const getTransactions = state => state.transactions.data;

export const getTransactionsByDays = createSelector(
  [getTransactions, getActiveCard],
  (transactions, activeCard) => separateByDates(transactions, activeCard)
);
