import React, { Fragment, useState, useEffect } from "react";
import PageTitle from "../../layouts/PageTitle";
import { Formik } from "formik";
import * as Yup from "yup";
import { useForm } from "react-hook-form";
import { getRoles } from "../../../services/RolesService";
import SelectField from "../../../form-controls/SelectField";
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import MenuItem from '@material-ui/core/MenuItem';
import InputField from "../../../form-controls/InputField";
import RadioField from "../../../form-controls/RadioField";
import FileField from "../../../form-controls/FileField";
import DateField from "../../../form-controls/DateField";
import { convertGenderToString, convertGenderToBoolean } from '../../../util/common';
import { createUser } from '../../../services/UsersService';
import { ToastContainer, toast } from 'react-toastify';
import { Link, useHistory, useLocation, useParams } from "react-router-dom";
import { useDispatch, useSelector } from 'react-redux';
import { createUserAction } from '../../../store/actions/UserActions';

const loginSchema = Yup.object().shape({
   username: Yup.string()
      .min(3, "Your username must consist of at least 3 characters ")
      .max(50, "Your username must consist of at least 3 characters ")
      .required("Please enter a username"),
   password: Yup.string()
      .min(5, "Your password must be at least 5 characters long")
      .max(50, "Your password must be at least 5 characters long")
      .required("Please provide a password"),
});

const CreateUser = () => {

   const dispatch = useDispatch();
   const [roles, setRoles] = useState([]);
   const [roleUser, setUserRole] = useState();

   const fetchRoles = async () => {
      try {

         const response = await getRoles()
         console.log("roles", response.data);
         
         if (response.data) {
            setRoles(response.data);
            let userrole = response.data.find(x => x.name === "User");
            if (userrole)
               setUserRole(userrole);
         }
         
      }
      catch (error) {
         console.log(error);
      }
   }



   useEffect(() => {

      fetchRoles()
   }, []);

   useEffect(() => {

      if (roleUser) {
         let defaultValues = {
            name: "",
            birthday: null,
            address: "",
            avatar: null,
            sex: "female",
            email: "",
            phoneNumber: "",
            roleID: roleUser.id
         }

         form.reset(defaultValues)
      }
   }, [roleUser])

   let history = useHistory();

   const phoneRegExp = /^((\+)33|0)[1-9](\d{2}){4}$/

   const schema = yup.object().shape({
      name: yup.string().required("Vui lòng nhập họ tên"),
      address: yup.string().required("Vui lòng nhập địa chỉ"),
      roleID: yup.string().required("Vui lòng lựa chọn quyền"),
      email: yup.string().required("Vui lòng nhập email").email("Email không hợp lệ"),
      phoneNumber: yup.string().required("Vui lòng nhập số điện thoại").matches(phoneRegExp, 'Số điện thoại không hợp lệ')
   })

   const form = useForm({
      defaultValues: {
         name: "",
         birthday: null,
         address: "",
         avatar: null,
         sex: "female",
         email: "",
         phoneNumber: "",
         roleID:""
      },
      resolver: yupResolver(schema)
   });

   function createFormData(data) {
      var formdata = new FormData();

      for (var key in data) {
         if (key !== 'image')
            formdata.append(key, data[key]);
         else if (fileSelected.selectedFile)
            formdata.append(key, fileSelected.selectedFile, fileSelected.selectedFile.name)
      }

      return formdata
   }

   const handleSubmit = (values) => {
      // values.id = user.id
      // if (!values.image)
      //    values.imageUrl = user.imageUrl
      values.gender = convertGenderToBoolean(values.sex)
      delete values.sex

      // handleUpdateUser(createFormData(values))
      handleCreateUser(createFormData(values))
      console.log("form", values)
   }

   let fileSelected = { selectedFile: null }

   function handleFileChange(e) {
      fileSelected = { selectedFile: e[0] }
   }

   async function handleCreateUser(user) {

      try {
         //const response = await createUser(user)
         dispatch(createUserAction(user, history))

         // console.log(response.data)
         // if (response.data.succeeded) {
         //    toast.success("Tạo tài khoản thành công", {
         //       type: toast.TYPE.SUCCESS,
         //       position: 'top-right',
         //       autoClose: 5000,
         //       hideProgressBar: false,
         //       closeOnClick: true,
         //       pauseOnHover: true,
         //       draggable: true,
         //       progress: undefined,
         //    })
         // }
         // else {
         //    toast.error("Tạo tài khoản thất bại",
         //       {
         //          position: 'top-right',
         //          autoClose: 5000,
         //          hideProgressBar: false,
         //          closeOnClick: true,
         //          pauseOnHover: true,
         //          draggable: true,
         //          progress: undefined
         //       }
         //    )
         // }
      }
      catch (error) {
         toast.error("Tạo tài khoản thất bại",
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



   return (
      <Fragment>
         <div className="form-head page-titles d-flex  align-items-center">
            <div className="mr-auto  d-lg-block">
               <h2 className="text-black font-w600">Quản lý người dùng</h2>
               <ol className="breadcrumb">
                  <li className="breadcrumb-item active">
                     <Link to="/customer-list">Người dùng</Link>
                  </li>
                  <li className="breadcrumb-item">
                     Tạo tài khoản
                  </li>
               </ol>
            </div>
         </div>
         
         <div className="row">
            <div className="col-lg-12">
               <div className="card">
                  <div className="card-header">
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
                  </div>
                  <div className="card-body">
                     <div className="form-validation">
                        <form
                           className="form-valide"
                           action="#"
                           method="post"
                           onSubmit={form.handleSubmit(handleSubmit)}
                        >
                           <div className="row">
                              <div className="col-xl-6">
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-email"
                                    >
                                       Họ tên{" "}
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       <InputField name="name" form={form} label="Họ tên"  />
                                    </div>
                                 </div>
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-username"
                                    >
                                       Ngày sinh
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       
                                       <DateField name="birthday" form={form} label="Ngày sinh" />
                                    </div>
                                 </div>
                                 
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-password"
                                    >
                                       Email
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       <InputField name="email" form={form} label="Email"  />
                                    </div>
                                 </div>
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-password"
                                    >
                                       Quyền
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       <SelectField
                                          id="trinity-select"
                                          name="roleID"
                                          label="Hãy lựa chọn quyền"
                                          form={form}
                                       >
                                          {
                                             roles.map((role) => (
                                                <MenuItem key={role.id} value={role.id}>
                                                   {role.name}
                                                </MenuItem>
                                             ))
                                          }
                                       </SelectField>
                                    </div>
                                 </div>
                              </div>
                              <div className="col-xl-6">
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-skill"
                                    >
                                       Avatar
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       <FileField name="avatar" form={form} handleFileChange={handleFileChange} />
                                       
                                    </div>
                                 </div>
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-currency"
                                    >
                                       Giới tính
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       <RadioField name="sex" form={form} />
                                    </div>
                                 </div>
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-website"
                                    >
                                       Số điện thoại
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       <InputField name="phoneNumber" form={form} label="Số điện thoại"  />
                                    </div>
                                 </div>
                                 <div className="form-group row">
                                    <label
                                       className="col-lg-4 col-form-label"
                                       htmlFor="val-phoneus"
                                    >
                                       Địa chỉ
                                       <span className="text-danger">*</span>
                                    </label>
                                    <div className="col-lg-7">
                                       <InputField name="address" form={form} label="Địa chỉ" />
                                    </div>
                                 </div>
                                 <div className="form-group row">
                                    <div className="col-lg-11">
                                       <button
                                          type="submit"
                                          className="btn btn-primary pull-right "
                                       >
                                          Tạo tài khoản
                                       </button>
                                       <button
                                          className="btn btn-danger pull-right mr-2"
                                          onClick={() => history.goBack()}
                                       >
                                          Quay lại
                                       </button>
                                    </div>
                                 </div>
                              </div>
                           </div>
                        </form>
                     </div>
                  </div>
               </div>
            </div>
         </div>
      </Fragment>
   );
};

export default CreateUser;
