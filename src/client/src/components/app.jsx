import React from "react";

import "./fonts.css";
import styled from '@emotion/styled';

import CardsBar from "./cards/cards_bar";
import Home from "./home/home";
import Header from "./header/header";

const Wallet = styled.div`
  min-width: 798px;
  min-height: 863px;
  display: grid;
  grid-template-rows: 74px 1fr;
  grid-template-columns: auto 1fr;
  background-color: #fcfcfc;
  width: 100%;
  margin: 0px auto;
  box-shadow: 0 2px 6px 0 rgba(0, 0, 0, 0.15);
`;

const CardsBarWrapper = styled.aside`
  background-color: #242424;
  grid-row: 1/-1;
  grid-column: 1;
  width: 310px;
`;

const HeaderWrapper = styled.header`
  grid-column: 2/-1;
  grid-row: 1;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.16);
`;

const HomeWrapper = styled.main`
  grid-row: 2;
  grid-column: 2;
`;

export default () => (
    <Wallet>
        <CardsBarWrapper>
            <CardsBar/>
        </CardsBarWrapper>
        <HeaderWrapper>
            <Header/>
        </HeaderWrapper>
        <HomeWrapper>
            <Home/>
        </HomeWrapper>
    </Wallet>
);
