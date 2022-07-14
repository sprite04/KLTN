import React from "react";
import { Link } from "react-router-dom";
import {
  Dropdown,
} from "react-bootstrap";

import Select from "@material-ui/core/Select";
import MenuItem from '@material-ui/core/MenuItem';
import { getRoles } from "../../../services/RolesService";

//** Import Image */
import avatar from "../../../images/avatar/avatar.png";
import customers4 from "../../../images/customers/4.jpg";
import customers3 from "../../../images/customers/3.jpg";
import customers2 from "../../../images/customers/2.jpg";
import customers1 from "../../../images/customers/1.jpg";
import { getUsersAction, updateLockUnlockUserAction } from '../../../store/actions/UserActions'
import { useDispatch, useSelector } from 'react-redux';
import { useEffect } from "react";
import { useRef } from "react";
import swal from "sweetalert";

import { useState } from "react";

import { formatDateTimeDDmmYYYY } from "../../../util/common";


import { makeStyles } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import InputBase from '@material-ui/core/InputBase';
import IconButton from '@material-ui/core/IconButton';
import { GetNews, UpdateStatusNews, deleteNews } from '../../../services/NewsService';
import { DataF } from "@react-google-maps/api";
import { ToastContainer, toast } from 'react-toastify';
import { Button } from "@material-ui/core";
import News from "./News";


const useStyles = makeStyles((theme) => ({
  root: {
    padding: '2px 4px',
    display: 'flex',
    alignItems: 'center',
    width: 400,
  },
  input: {
    marginLeft: theme.spacing(1),
    flex: 1,
  },
  iconButton: {
    padding: 10,
  },
  divider: {
    height: 28,
    margin: 4,
  },
}));

const displayStatus = [
  { id: true, text: "Đang hiển thị" },
  { id: false, text: "Không hiển thị" }
]


function NewsList() {
  const dispatch = useDispatch();


  const classes = useStyles();








  //---------------------------------------------------

  const size = 10
  const activePag = useRef(1)
  const [news, setNews] = useState([]);
  const [totalSize, setTotalSize] = useState(0);


  const fetchNews = async (data) => {
    try {

      const response = await GetNews(data)

      setNews(response.data.news);
      setTotalSize(response.data.totalSize);

      console.log("news", response.data);
    }
    catch (error) {
      console.log(error);
    }
  }

  const fetchUpdateStatusNews = async (id, status) => {
    try {

      const response = await UpdateStatusNews(id, status);

      console.log("news", response.data);

      if (response.data) {
        setNews((news) => {
          let newsList = [...news]
          let value = newsList.find(n => n.id === id);
          value.display = status;


          console.log(value)

          return newsList;
        })
        toast.success("Thực hiện thành công", {
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
      console.log(error);
      toast.error("Thực hiện thất bại",
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

  const fetchDeleteNews = async (id) => {
    try {

      console.log(id)
      const response = await deleteNews(id)


      console.log("news", response.data);

      //console.log(response.data);
      if (response.data === true) {
        setNews((news) => {
          let newsList = [...news]
          let value = newsList.findIndex(n => n.id === id);
          newsList.splice(value, 1)

          return newsList;
        })

        toast.success("Thực hiện thành công", {
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
      console.log(error);
      toast.error("Thực hiện thất bại",
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

  function handleDeleteNews(id) {
    fetchDeleteNews(id)

  }

  function handleUpdateNewsStatus(id, display) {
    
    let status = !display;
    console.log("meomeeo...", status)
    fetchUpdateStatusNews(id, status)
  }


  useEffect(() => {

    const data = {
      page: activePag.current,
      size: size
    }

    fetchNews(data)
  }, [])

  let paggination = Array(Math.ceil(totalSize / size))
    .fill()
    .map((_, i) => i + 1)

  const keyWord = useRef('');

  function handleFind(event) {
    let input = document.getElementById("search-input");

    keyWord.current = input.value.trim()
    activePag.current = 1

    handleSubmit()
  }

  function handleInput(event) {

    if (event.target.value === '' && keyWord.current !== '') {
      keyWord.current = event.target.value
      activePag.current = 1

      handleSubmit()
    }
  }

  const status = useRef('default');

  function handleSelectRole(event) {
    status.current = event.target.value
    activePag.current = 1

    handleSubmit()
  }

  function handleSubmit() {
    let data = {
      page: activePag.current,
      size: size
    }
    if (status.current !== 'default')
      data.display = status.current
    if (keyWord.current !== '')
      data.search = keyWord.current

    fetchNews(data)
  }

  const onClick = (i) => {
    activePag.current = i
    handleSubmit()
  }


  return (
    <>

      <div className="form-head page-titles d-flex  align-items-center">
        <div className="mr-auto  d-lg-block">
          <h2 className="text-black font-w600">Quản lý tin tức</h2>
          <ol className="breadcrumb">
            <li className="breadcrumb-item active">
              <Link to="/order-list">Tin tức</Link>
            </li>
            <li className="breadcrumb-item">
              Danh sách tin tức
            </li>
          </ol>
        </div>
        <Link
          to="/news"
          className="btn btn-primary rounded light mr-3"
        >
          Tạo tin tức
        </Link>
      </div>

      <div className="row">
        <div className="col-xl-12">
          <div className="row">
            <div className="col-xl-4">
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
              <Paper className={classes.root}>
                <InputBase
                  id="search-input"
                  className={classes.input}
                  placeholder="Tìm kiếm"
                  onChange={handleInput}
                />
                <IconButton type="submit" className={classes.iconButton} aria-label="search" onClick={handleFind}>
                  <i class="las la-search"></i>
                </IconButton>
              </Paper>
            </div>
            <div className="col-xl-3">
              <Select defaultValue="default" name="role" fullWidth variant="outlined" style={{ color: '#7e7e7e', backgroundColor: '#fff', padding: '0', height: '49px' }} onChange={handleSelectRole}>
                <MenuItem value="default">
                  {"Tình trạng hiển thị"}
                </MenuItem>
                {

                  displayStatus.map((status) => (
                    <MenuItem key={status.id} value={status.id}>
                      {status.text}
                    </MenuItem>
                  ))
                }
              </Select>
            </div>

          </div>

          <div className='table-responsive table-hover fs-14 mt-3'>

            <div id='orderList' className='dataTables_wrapper no-footer'>

              <table
                className='table table-hover display mb-4 dataTablesCard card-table dataTable no-footer'
                id='example5'
              >
                <thead>
                  <tr role='row'>
                    <th className='sorting'></th>
                    <th className='sorting'>Ngày tạo</th>
                    <th className='sorting'>Tiêu đề</th>

                    <th >Người tạo</th>
                    <th className='sorting'>Tình trạng</th>
                    <th className='sorting' />
                  </tr>
                </thead>
                <tbody>
                  {
                    news.map((n) => {

                      return (<tr role='row' className='odd' key={n.id}>
                        <td className='text-ov'>{n.id}</td>
                        <td>{formatDateTimeDDmmYYYY(n.createdDate)}</td>
                        <td className='text-ov'>{n.title}</td>

                        <td>{n.creatorName}</td>
                        <td>{n.display ? "Đang hiển thị" : "Không hiển thị"}</td>
                        <td>
                          <Dropdown className='ml-auto'>
                            <Dropdown.Toggle
                              variant=''
                              className='btn-link i-false'
                            >
                              <svg
                                width={24}
                                height={24}
                                viewBox='0 0 24 24'
                                fill='none'
                                xmlns='http://www.w3.org/2000/svg'
                              >
                                <path
                                  d='M11.0005 12C11.0005 12.5523 11.4482 13 12.0005 13C12.5528 13 13.0005 12.5523 13.0005 12C13.0005 11.4477 12.5528 11 12.0005 11C11.4482 11 11.0005 11.4477 11.0005 12Z'
                                  stroke='#3E4954'
                                  strokeWidth={2}
                                  strokeLinecap='round'
                                  strokeLinejoin='round'
                                />
                                <path
                                  d='M18.0005 12C18.0005 12.5523 18.4482 13 19.0005 13C19.5528 13 20.0005 12.5523 20.0005 12C20.0005 11.4477 19.5528 11 19.0005 11C18.4482 11 18.0005 11.4477 18.0005 12Z'
                                  stroke='#3E4954'
                                  strokeWidth={2}
                                  strokeLinecap='round'
                                  strokeLinejoin='round'
                                />
                                <path
                                  d='M4.00049 12C4.00049 12.5523 4.4482 13 5.00049 13C5.55277 13 6.00049 12.5523 6.00049 12C6.00049 11.4477 5.55277 11 5.00049 11C4.4482 11 4.00049 11.4477 4.00049 12Z'
                                  stroke='#3E4954'
                                  strokeWidth={2}
                                  strokeLinecap='round'
                                  strokeLinejoin='round'
                                />
                              </svg>
                            </Dropdown.Toggle>
                            <Dropdown.Menu className='dropdown-menu-right'>
                              <Dropdown.Item className='text-black'>
                                <Link to={`news/${n.id}`} className="text-black">
                                  Chỉnh sửa
                                </Link>

                              </Dropdown.Item>
                              <Dropdown.Item className='text-black' onClick={() => handleUpdateNewsStatus(n.id, n.display)}>
                                {n.display ? "Ẩn tin" : "Hiển thị tin"}
                              </Dropdown.Item>
                              <Dropdown.Item className='text-black' onClick={() => {
                                
                                swal({
                                  title: "Bạn có muốn xoá tin này",
                                  icon: "warning",
                                  buttons: true,
                                  dangerMode: true,
                                }).then((accept) => {
                                  if (accept) {
                                    handleDeleteNews(n.id)
                                  }
                                })
                              }} >
                                Xoá
                              </Dropdown.Item>
                            </Dropdown.Menu>
                          </Dropdown>
                        </td>
                      </tr>)
                    })
                  }
                </tbody>
              </table>
              <div className='d-sm-flex text-center justify-content-between align-items-center mt-3'>
                <div className='dataTables_info'>
                </div>
                <div
                  className='dataTables_paginate paging_simple_numbers'
                  id='example5_paginate'
                >
                  <Link
                    className='paginate_button previous disabled'
                    to='/news-list'
                    onClick={() =>
                      activePag.current > 0 && onClick(activePag.current - 1)
                    }
                  >
                    Trước
                  </Link>
                  <span>
                    {paggination.map((number, i) => (
                      <Link
                        key={i}
                        to='/news-list'
                        className={`paginate_button  ${activePag.current === number ? 'current' : ''
                          } `}
                        onClick={() => onClick(number)}
                      >
                        {number}
                      </Link>
                    ))}
                  </span>
                  <Link
                    className='paginate_button next'
                    to='/news-list'
                    onClick={() =>
                      activePag.current < paggination.length &&
                      onClick(activePag.current + 1)
                    }
                  >
                    Sau
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default NewsList;
