import React from 'react';
import PropTypes from 'prop-types';
import styled from '@emotion/styled';

const OverlayLayout = styled.div`
	position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
	background: rgba(0, 0, 0, 0.7);
`;

const CloseLayout = styled.div`
  position: absolute;
  top: -10px;
  right: 5px;
  font-size: 30px;
  cursor: pointer;
`;

const PopupLayout = styled.div`
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  box-sizing: border-box;
  width: 350px;
  margin: 40px auto 0 auto;
	padding: 20px;
	border-radius: 4px;
	background: #fff;
`;

const Popup = ({children, className, onCloseClick}) => (
    <OverlayLayout>
        <PopupLayout className={className}>
            <CloseLayout onClick={onCloseClick}>×</CloseLayout>
            {children}
        </PopupLayout>
    </OverlayLayout>
);

Popup.propTypes = {
    children: PropTypes.node,
    className: PropTypes.string
};

export default Popup;