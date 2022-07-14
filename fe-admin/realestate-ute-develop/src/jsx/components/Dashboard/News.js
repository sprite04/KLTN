import React, { Component, useRef, useState } from "react";
import Rte from "./Rte";
import PageTitle from "../../layouts/PageTitle";
import InputField from "../../../form-controls/InputField";
import * as yup from 'yup';
import { useForm } from "react-hook-form";
import { yupResolver } from '@hookform/resolvers/yup';
import FileField from "../../../form-controls/FileField";
import EditorField from "../../../form-controls/EditorField";
import { Editor } from "@tinymce/tinymce-react";
import { TextField } from "@material-ui/core";
import { createNews, updateNews } from "../../../services/NewsService";
import { useDispatch, useSelector } from 'react-redux';
import { ToastContainer, toast } from 'react-toastify';
import { Link, useParams } from "react-router-dom";
import { GetNewsById } from "../../../services/NewsService";
import { useEffect } from 'react';
import { useHistory, useLocation } from "react-router-dom";

let fileSelected = { selectedFile: null }

function News() {

   let history = useHistory();
   let param = useParams();
   let newId = param.newId;
   console.log(newId)

   const editorRef = useRef('');
   const titleRef = useRef(null);

   const userInfo = useSelector(state => state.auth.auth);

   const [newsOld, setNews] = useState();



   const fetchNews = async (id) => {
      try {

         const response = await GetNewsById(id)

         if (response.data) {
            setNews(response.data)
            
            titleRef.current.value = response.data.title

            console.log("teoteo",editorRef.current)
            console.log(editorRef.current)
            if (editorRef.current) {
               console.log("memeo")
               editorRef.current.setContent(response.data.details); 
            }
               

         }

         // setNews(response.data.news);
         // setTotalSize(response.data.totalSize);

         console.log("news", response.data);
      }
      catch (error) {
         console.log(error);
      }
   }



   useEffect(() => {

      
      if (newId)
         fetchNews(newId)
   }, [])
   

   function handleFileChange(e) {
      fileSelected.selectedFile = e[0]
   }

   

   const [titleInvalid, setTitleInvalid] = useState(false)
   const [descriptionInvalid, setDescriptionInvalid] = useState(false)

   async function handleCreateNews(news) {

      try {
         const response = await createNews(news)
         if (response.data) {
            toast.success("Tạo tin tức thành công", {
               type: toast.TYPE.SUCCESS,
               position: 'top-right',
               autoClose: 5000,
               hideProgressBar: false,
               closeOnClick: true,
               pauseOnHover: true,
               draggable: true,
               progress: undefined,
            })
            history.goBack()
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
         toast.error("Tạo tin tức thất bại",
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


   async function handleUpdateNews(news) {
      
      news.append("id", newId);
      if (!fileSelected.selectedFile && newsOld.imageUrl)
         news.append("imageUrl", newsOld.imageUrl);
         
      try {
         const response = await updateNews(news)
         if (response.data) {
            toast.success("Cập nhật tin tức thành công", {
               type: toast.TYPE.SUCCESS,
               position: 'top-right',
               autoClose: 5000,
               hideProgressBar: false,
               closeOnClick: true,
               pauseOnHover: true,
               draggable: true,
               progress: undefined,
            })
            history.goBack()
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
         toast.error("Cập nhật tin tức thất bại",
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




   const handleSubmit = (values) => {

      let valid = true
      let titleInput = document.getElementById("titleInput")
      if (titleInput.value === "")
      {
         valid = false
         setTitleInvalid(true)
      }
      else
         setTitleInvalid(false)

      let editorContent = editorRef.current.getContent()
      if (editorContent === "")
      {
         valid = false
         setDescriptionInvalid(true)
      }
      else
         setDescriptionInvalid(false)
      
      if (!valid)
         return
      
      let data = {
         title: titleInput.value,
         details: editorContent,
         creatorID: userInfo.id
      }
      
      //console.log(createFormData(data))
      if (newId)
         handleUpdateNews(createFormData(data))
      else
         handleCreateNews(createFormData(data))

      
   }

   function createFormData(data) {
      var formdata = new FormData();

      for (var key in data) {
         formdata.append(key, data[key]);
            
      }
      console.log("meoemeoe", fileSelected.selectedFile)
      if (fileSelected.selectedFile)
      {
         formdata.append("image", fileSelected.selectedFile, fileSelected.selectedFile.name)
         
      }
         
      

      return formdata
   }


   return (<>
      <div className="h-80">
         
         <div className="form-head page-titles d-flex  align-items-center">
            <div className="mr-auto  d-lg-block">
               <h2 className="text-black font-w600">Quản lý tin tức</h2>
               <ol className="breadcrumb">
                  <li className="breadcrumb-item active">
                     <Link to="/order-list">Tin tức</Link>
                  </li>
                  <li className="breadcrumb-item">
                     {newId ? "Cập nhật tin tức" : "Tạo tin tức"}
                  </li>
               </ol>
            </div>
         </div>
         <div className="row">
            <div className="col-xl-12 col-xxl-12">
               <div className="card">
                  <div className="card-header">
                     {/* <h4 className="card-title">{newId? "Cập nhật tin tức": "Tạo tin tức"}</h4> */}
                  </div>
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
                  <div className="card-body">
                     <div className="row">
                        <div className="col-xl-9">
                           Tiêu đề:
                        </div>
                        <div className="col-xl-3">
                           Ảnh bìa:
                        </div>

                     </div>
                     <div className="row">
                        <div className="col-xl-9">
                           <TextField id="titleInput" name="title" fullWidth variant="outlined" inputRef={titleRef} />
                           {titleInvalid && (<div
                              id="val-username1-error"
                              className="invalid-feedback animated fadeInUp"
                              style={{ display: "block" }}
                           >
                              Vui lòng nhập tiêu đề
                           </div>)}
                        </div>
                        <div className="col-xl-3">
                           <TextField
                              type="file"
                              fullWidth
                              variant="outlined"
                              onChange={(e) => handleFileChange(e.target.files)}
                           //inputRef={titleRef} // wire up the input ref
                           />
                        </div>

                     </div>
                     <div className="row mt-3">
                        <div className="col-xl-12">
                           Nội dung:
                        </div>
                     </div>
                     <div className="summernote ">

                        <Editor
                           onInit={(evt, editor) => {
                              editorRef.current = editor
                              if (newId)
                                 fetchNews(newId)
                           }}
                           init={{
                              height: 900,
                              menubar: false,
                              plugins: [
                                 "advlist autolink lists link image code charmap print preview anchor",
                                 "searchreplace visualblocks code fullscreen",
                                 "insertdatetime media table paste code help wordcount",
                              ],
                              toolbar:
                                 "undo redo | formatselect | code |link | image | bold italic backcolor | alignleft aligncenter alignright alignjustify |  \n" +
                                 "bullist numlist outdent indent | removeformat | help ",
                              content_style: 'body { color: #7e7e7e }'
                           }}
                           
                        />
                        {descriptionInvalid && (<div
                           id="val-username1-error"
                           className="invalid-feedback animated fadeInUp"
                           style={{ display: "block" }}
                        >
                           Vui lòng nhập mô tả
                        </div>)}

                     </div>
                     <div className='form-group row mt-3'>
                        <div className='col-sm-12'>

                           <button type="button" onClick={ handleSubmit } className='btn btn-primary pull-right'>
                              {newId?"Cập nhật": "Tạo tin"}
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
                     
                  </div>
               </div>
            </div>
         </div>
      </div>
   </>
   )
}

export default News;
