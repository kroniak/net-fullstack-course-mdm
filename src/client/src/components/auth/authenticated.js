export default (props) => {
    if (props.isAuth && props.children) return props.children;
    else return null;
};