import "./index.css"
import React from "react";
import ReactDOM from "react-dom";
// Redux
import {Provider} from "react-redux";
import {applyMiddleware, compose, createStore} from "redux";
import reduxThunk from "redux-thunk";
// Components
import App from "./components/app";

import reducers from "./reducers";

const middleware = [reduxThunk];

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
const store = createStore(
    reducers,
    composeEnhancers(applyMiddleware(...middleware))
);

ReactDOM.render(<Provider store={store}>
        <App/>
    </Provider>,
    document.getElementById("root")
);
