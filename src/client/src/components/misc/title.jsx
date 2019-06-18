import React from 'react';
import PropTypes from 'prop-types';
import styled from '@emotion/styled';

const StyledTitle = styled.h2`
	margin-top: 12px;
	font-size: 24px;
	font-weight: 600;
	color: #000;
`;

const Title = ({children, className}) => (
    <StyledTitle className={className}>
        {children}
    </StyledTitle>
);

Title.propTypes = {
    children: PropTypes.node,
    className: PropTypes.string
};

export default Title;
