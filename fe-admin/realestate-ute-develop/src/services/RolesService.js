import axiosInstance from './AxiosInstance';

export function getRoles() {

    return axiosInstance.get(`UserManagements/GetAllRoles`);
}
