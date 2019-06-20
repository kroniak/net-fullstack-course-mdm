export default (props) => {
    if (props.isAuth) return props.children;
    else return null;
};