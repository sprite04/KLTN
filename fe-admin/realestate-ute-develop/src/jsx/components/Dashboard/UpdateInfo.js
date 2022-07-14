import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";

//** Import Image */
import avatar from "../../../images/avatar/avatar.png";
import map from "../../../images/map.jpg";
import customers10 from "../../../images/customers/10.jpg";
import customers11 from "../../../images/customers/11.jpg";
import customers12 from "../../../images/customers/12.jpg";

import { formatDateTimeDDmmYYYY, formatDateTimeYYYYmmdd } from "../../../../src/util/common";

import { getUserById, lockunlock, updateUser, changePassword } from '../../../services/UsersService';
import { Card, Tab } from 'react-bootstrap'
import swal from "sweetalert";
import { useDispatch, useSelector } from 'react-redux';
import { confirmLockUnlockUserAction, confirmedUpdateUserAction } from '../../../store/actions/UserActions';
import { Nav } from 'react-bootstrap';
import { getPosts } from '../../../services/PostsService';
import { useRef } from "react";
import { useForm } from "react-hook-form";
import InputField from "../../../form-controls/InputField.js";
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';

import DateField from "../../../form-controls/DateField";
import FileField from "../../../form-controls/FileField";
import RadioField from "../../../form-controls/RadioField";
import { convertGenderToString, convertGenderToBoolean } from '../../../util/common';
import { useHistory, useLocation } from "react-router-dom";

import { ToastContainer, toast } from 'react-toastify';

function UpdateInfo() {

    const dispatch = useDispatch();


    let history = useHistory();


    const userInfo = useSelector(state => state.auth.auth)

    console.log("admin_____", userInfo)





    let param = useParams();
    let userId = param.userId;


    if (userId && userInfo.rolename !== "Admin") {
        history.push('/');
    }
        
    if (!userId && userInfo) {
        userId = userInfo.id
    }


    const sort = 4;
    const activePag = useRef(1);


    const [user, setUser] = useState({});

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await getUserById(userId);
                console.log(response)
                setUser(response.data)
            }
            catch (error) {
                console.log(error)
            }
        }

        fetchUser()
    }, []);

    


    async function handleLockUnlock(userId) {

        try {
            const response = await lockunlock(userId)
            dispatch(confirmLockUnlockUserAction(userId, response.data.lock))

            setUser(response.data)
        }
        catch (error) {
            console.log(error)
        }
    }



    const phoneRegExp = /^((\+)33|0)[1-9](\d{2}){4}$/
    
    const schema = yup.object().shape({
        name: yup.string().required("Vui lòng nhập họ tên"),
        address: yup.string().required("Vui lòng nhập địa chỉ"),
        email: yup.string().required("Vui lòng nhập email").email("Email không hợp lệ"),
        phoneNumber: yup.string().required("Vui lòng nhập số điện thoại").matches(phoneRegExp, 'Số điện thoại không hợp lệ')
    })

    const form = useForm({
        defaultValues: {
            name: "",
            birthday: null,
            address: "",
            image: null,
            sex: false,
            email: "",
            phoneNumber: ""
        },
        resolver: yupResolver(schema)
    });

    const schemaPassword = yup.object().shape({
        oldPassword: yup.string().required("Vui lòng nhập mật khẩu cũ"),
        password: yup.string().required("Vui lòng nhập mật khẩu mới"),
        confirmPassword: yup.string().required("Vui lòng xác thực mật khẩu mới")
    });

    const formPassword = useForm({
        defaultValues: {
            oldPassword: "",
            password: "",
            confirmPassword: ""
        },
        resolver: yupResolver(schemaPassword)
    });



    useEffect(() => {
        if (user) {
            let birthday = formatDateTimeYYYYmmdd(user.birthday)
            let sex = convertGenderToString(user.gender)

            let defaultValues = {
                name: user.name,
                birthday: birthday,
                address: user.address,
                image: null,
                sex: sex,
                email: user.email,
                phoneNumber: user.phoneNumber
            }
            
            form.reset(defaultValues)
        }
        
        
    }, [user]);

    async function handleUpdateUser(user) {

        try {
            const response = await updateUser(user)
            dispatch(confirmedUpdateUserAction(response.data.user))
            

            setUser(response.data.user)
            toast.success("Thay đổi thông tin thành công", {
                type: toast.TYPE.SUCCESS,
                position: 'top-right',
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            })
        }
        catch (error) {
            toast.error("Thay đổi thông tin thất bại",
                {
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined
                }
            )
        }
    }

    async function handleUpdatePasswordUser(user) {

        try {
            const response = await changePassword(user)
            if (response.data.succeeded) {
                toast.success("Thay đổi mật khẩu thành công", {
                    type: toast.TYPE.SUCCESS,
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                })
            }
            else {
                toast.error(response.data.errors,
                    {
                        position: 'top-right',
                        autoClose: 5000,
                        hideProgressBar: false,
                        closeOnClick: true,
                        pauseOnHover: true,
                        draggable: true,
                        progress: undefined
                    }
                )
            }

            
        }
        catch (error) {
            toast.error("Thay đổi mật khẩu thất bại",
                {
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined
                }
            )
        }
        let defaultValues = {
            oldPassword: "",
            password: "",
            confirmPassword: ""
        };
        formPassword.reset(defaultValues)
    }

    function createFormData(data) {
        var formdata = new FormData();

        for (var key in data) {
            if (key !== 'image')
                formdata.append(key, data[key]);
            else if (fileSelected.selectedFile)
                formdata.append(key, fileSelected.selectedFile, fileSelected.selectedFile.name )
        }

        return formdata
    }

    const handleSubmit = (values) => {
        values.id = user.id
        if (!values.image)
            values.imageUrl = user.imageUrl
        values.gender = convertGenderToBoolean(values.sex)
        delete values.sex

        handleUpdateUser(createFormData(values))
    }

    const handlePasswordSubmit = (values) => {
        console.log(values)
        if (values.oldPassword === values.password) {
            toast.error("Mật khẩu mới không được trùng với mật khẩu cũ.",
                {
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined
                }
            )
        }
        else if (values.password !== values.confirmPassword) {
            toast.error("Mật khẩu xác thực không khớp",
                {
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined
                }
            )
        }
        else {
            handleUpdatePasswordUser(values)
        }
    }

    
    let fileSelected = { selectedFile: null }

    function handleFileChange(e) {
        fileSelected = { selectedFile: e[0] }
    }



    return (
        <>
            <div className="form-head page-titles d-flex  align-items-center">
                <div className="mr-auto  d-lg-block">
                    <h2 className="text-black font-w600">Quản lý người dùng</h2>
                    <ol className="breadcrumb">
                        <li className="breadcrumb-item active">
                            <Link to="/customer-list">Người dùng</Link>
                        </li>
                        <li className="breadcrumb-item">
                            Cập nhật thông tin
                        </li>
                    </ol>
                </div>
            </div>
            <div className="row">
                <div className="col-xl-3 col-xxl-4">
                    <div className="row">

                        <div className="col-xl-12 col-lg-12">
                            <div className="card text-center">
                                <div className="card-body">
                                    <div className="position-relative mb-3 d-inline-block">
                                        <img src={user.imageUrl ? user.imageUrl : avatar} alt="" className="rounded" width={140} height={140} style={{ objectFit: "cover" }} />
                                        <div className="profile-icon"
                                            onClick={() => {
                                                let title = ""
                                                if (user.lock)
                                                    title = `Bạn có muốn mở khoá người dùng ${user.name} - ${user.userName}?`
                                                else
                                                    title = `Bạn có muốn khoá người dùng ${user.name} - ${user.userName}?`
                                                swal({
                                                    title: title,
                                                    icon: "warning",
                                                    buttons: true,
                                                    dangerMode: true,
                                                }).then((willLockUnlock) => {
                                                    if (willLockUnlock) {
                                                        handleLockUnlock(user.id)
                                                    }
                                                })
                                            }

                                            }
                                        >
                                            {
                                                user.lock ? (<i class="las la-lock"></i>) : (<i class="las la-unlock"></i>)
                                            }
                                        </div>
                                    </div>
                                    <h4 className="text-black fs-20 font-w600">
                                        {user.name}
                                    </h4>
                                    <span className="mb-3 text-black d-block">{user.roleName}</span>
                                    <p>
                                        {user.gender ? (<i class="las la-venus"></i>) : (<i class="las la-mars"></i>)}  {formatDateTimeDDmmYYYY(user.birthday)}
                                    </p>
                                    <p>
                                        {user.address}
                                    </p>
                                </div>
                                <div className="card-footer border-0 pt-0">
                                    {
                                        user.phoneNumber && (<div
                                            className="btn btn-outline-primary d-block rounded"
                                        >
                                            <i className="las la-phone scale5 mr-2" />
                                            {user.phoneNumber}
                                        </div>)
                                    }
                                    {
                                        user.email && (<div
                                            className="btn btn-outline-primary rounded  mt-2"
                                        >
                                            {user.email}
                                        </div>)
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-xl-9 col-xxl-8">
                    <div className="row">
                        <div className="col-xl-12">
                            <div>
                                <ToastContainer
                                    position='top-right'
                                    autoClose={5000}
                                    hideProgressBar={false}
                                    newestOnTop
                                    closeOnClick
                                    rtl={false}
                                    pauseOnFocusLoss
                                    draggable
                                    pauseOnHover
                                />
                                <Card>
                                    <Card.Header className='d-block card-header'>
                                        <Card.Title>Thông tin cá nhân</Card.Title>
                                    </Card.Header>
                                    <Card.Body className='card-body'>
                                        <div className='default-tab'>
                                            <Tab.Container defaultActiveKey={"Home".toLowerCase()}>
                                                <Nav as='ul' className='nav-tabs'>
                                                    <Nav.Item as='li' >
                                                        <Nav.Link eventKey={"Home".toLowerCase()}>
                                                            {"Thay đổi thông tin"}
                                                        </Nav.Link>
                                                    </Nav.Item>
                                                    {
                                                        userId === userInfo.id ? (<Nav.Item as='li' >
                                                            <Nav.Link eventKey={"ChangePassword".toLowerCase()}>
                                                                {"Thay đổi mật khẩu"}
                                                            </Nav.Link>
                                                        </Nav.Item>) : ""
                                                    }
                                                    
                                                </Nav>
                                                <Tab.Content className='pt-4'>
                                                    <Tab.Pane eventKey={"Home".toLowerCase()} >
                                                        <div className='basic-form'>
                                                            <form onSubmit={form.handleSubmit(handleSubmit)}>
                                                                <div className='form-group row'>
                                                                    <label className='col-sm-3 col-form-label'>Họ tên</label>
                                                                    <div className='col-sm-9'>
                                                                        <InputField name="name" form={form} label="Họ tên" required={true} />
                                                                    </div>
                                                                </div>
                                                                <fieldset className='form-group'>
                                                                    <div className='row'>
                                                                        <label className='col-form-label col-sm-3 pt-0'>
                                                                            Giới tính
                                                                        </label>
                                                                        <div className='col-sm-9'>
                                                                            <RadioField name="sex" form={form}  />
                                                                        </div>
                                                                    </div>
                                                                </fieldset>
                                                                <div className='form-group row'>
                                                                    <label className='col-sm-3 col-form-label'>Ngày sinh</label>
                                                                    <div className='col-sm-9'>
                                                                        
                                                                        <DateField name="birthday" form={form} label="Ngày sinh"/>
                                                                    </div>
                                                                </div>
                                                                <div className='form-group row'>
                                                                    <label className='col-sm-3 col-form-label'>Email</label>
                                                                    <div className='col-sm-9'>
                                                                        <InputField name="email" form={form} label="Email" required={true} />
                                                                    </div>
                                                                </div>
                                                                <div className='form-group row'>
                                                                    <label className='col-sm-3 col-form-label'>Số điện thoại</label>
                                                                    <div className='col-sm-9'>
                                                                        <InputField name="phoneNumber" form={form} label="Số điện thoại" required={true} />
                                                                    </div>
                                                                </div>
                                                                <div className='form-group row'>
                                                                    <label className='col-sm-3 col-form-label'>Địa chỉ</label>
                                                                    <div className='col-sm-9'>
                                                                        <InputField name="address" form={form} label="Địa chỉ" />
                                                                    </div>
                                                                </div>
                                                                <div className='form-group row'>
                                                                    <label className='col-sm-3 col-form-label'>Avatar</label>
                                                                    <div className='col-sm-9'>
                                                                        <FileField name="image" form={form} handleFileChange={handleFileChange} />
                                                                    </div>
                                                                </div>



                                                                <div className='form-group row'>
                                                                    <div className='col-sm-12'>
                                                                        <button type='submit' className='btn btn-primary pull-right'>
                                                                            Thay đổi
                                                                        </button>

                                                                        <button
                                                                            type="button"
                                                                            className="btn btn-danger pull-right mr-2"
                                                                            onClick={() => history.goBack()}
                                                                        >
                                                                            Quay lại
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                            </form>
                                                        </div>
                                                    </Tab.Pane>
                                                    {
                                                        userId === userInfo.id ? (<Tab.Pane eventKey={"ChangePassword".toLowerCase()} >
                                                            <div className='basic-form'>
                                                                <form onSubmit={formPassword.handleSubmit(handlePasswordSubmit)}>
                                                                    <div className='form-group row'>
                                                                        <label className='col-sm-4 col-form-label'>Mật khẩu cũ</label>
                                                                        <div className='col-sm-8'>
                                                                            <InputField name="oldPassword" form={formPassword} required={true} type="password" />
                                                                        </div>
                                                                    </div>
                                                                    <div className='form-group row'>
                                                                        <label className='col-sm-4 col-form-label'>Mật khẩu mới</label>
                                                                        <div className='col-sm-8'>
                                                                            <InputField name="password" form={formPassword} required={true} type="password" />
                                                                        </div>
                                                                    </div>
                                                                    <div className='form-group row'>
                                                                        <label className='col-sm-4 col-form-label'>Xác nhận mật khẩu mới</label>
                                                                        <div className='col-sm-8'>
                                                                            <InputField name="confirmPassword" form={formPassword} required={true} type="password" />
                                                                        </div>
                                                                    </div>
                                                                    <div className='form-group row'>
                                                                        <div className='col-sm-12'>
                                                                            <button type='submit' className='btn btn-primary pull-right'>
                                                                                Thay đổi
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                </form>
                                                            </div>

                                                        </Tab.Pane>) : ""
                                                    }
                                                    
                                                </Tab.Content>
                                            </Tab.Container>
                                        </div>
                                    </Card.Body>
                                </Card>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </>
    );
}

export default UpdateInfo;
