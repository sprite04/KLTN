export const isAuthenticated = (state) => {
    //thêm xử lý tại đây để check có auth và còn hạn hay không
    if (state.auth.auth.idToken) return true;
    return false;
};
