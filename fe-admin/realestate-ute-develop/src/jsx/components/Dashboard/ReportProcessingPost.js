import React, { useEffect, useRef, useState } from "react";

import PageTitle from "../../layouts/PageTitle";

import { Button, Dropdown } from "react-bootstrap";
import { Link, useLocation } from "react-router-dom";
import { getReports } from "../../../services/ReportsService";
import { getReportProcessing, blockAllPosts } from "../../../services/ReportProcessingService";
import { addLeadingZeros, formatDateTimeDDmmYYYY } from "../../../util/common";
import { Nav, Pagination } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import { Alert } from 'react-bootstrap';

const ReportProcessingPost = () => {
  

  // const motherChackBox = document.querySelector(".product_order_single");
  // const chackbox = document.querySelectorAll(".product_order");
  // const chackboxFun = (type) => {
  //   for (let i = 0; i < chackbox.length; i++) {
  //     const element = chackbox[i];
  //     if (type === "all") {
  //       if (motherChackBox.checked) {
  //         element.checked = true;
  //       } else {
  //         element.checked = false;
  //       }
  //     } else {
  //       if (!element.checked) {
  //         motherChackBox.checked = false;
  //         break;
  //       } else {
  //         motherChackBox.checked = true;
  //       }
  //     }
  //   }
  // };


  //----------------------------------------------------------------------------------------------------------
  const [reportProcessings, setReportProcessings] = useState([]);
  const [totalSize, setTotalSize] = useState(0);

  const size = 4;
  const activePag = useRef(1);

  const fetchReportProcessings = async (page) => {
    try {

      const response = await getReportProcessing({
        page: page,
        size: size
      })

      setReportProcessings(response.data.reportProcessings);
      setTotalSize(response.data.totalSize);

      console.log("reportprocessings", response.data);
    }
    catch (error) {
      console.log(error);
    }
  }


  useEffect(() => {

    fetchReportProcessings(activePag.current)
  }, [])

  let paggination = Array(Math.ceil(totalSize / size))
    .fill()
    .map((_, i) => i + 1)
  
  const onClick = (i) => {
    activePag.current = i
    chageData(activePag.current)
  }

  const chageData = (page) => {
    fetchReportProcessings(page)

  }

  const pag = (size, gutter, variant, bg, circle) => (
    <Pagination

      className={`mt-4  ${gutter ? 'pagination-gutter' : ''} ${variant && `pagination-${variant}`
        } ${!bg && 'no-bg'} ${circle && 'pagination-circle'}`}
    >
      <li className='page-item page-indicator'>
        <Link className='page-link' to='#' onClick={() =>
          activePag.current > 0 && onClick(activePag.current - 1)
        }>
          <i className='la la-angle-left' />
        </Link>
      </li>
      {
        paggination.map((number, i) => (
          <Pagination.Item key={number} active={number === activePag.current} onClick={() => onClick(number)}>
            {number}
          </Pagination.Item>

        ))
      }
      <li className='page-item page-indicator'>
        <Link className='page-link' to='#' onClick={() =>
          activePag.current < paggination.length &&
          onClick(activePag.current + 1)
        }>
          <i className='la la-angle-right' />
        </Link>
      </li>
    </Pagination>
  )

  const fetchBlockAllPosts = async () => {
    try {

      const response = await blockAllPosts()

      // setReportProcessings(response.data.reportProcessings);
      // setTotalSize(response.data.totalSize);
      if (response.data) {
        toast.success("Thực hiện thay đổi thành công", {
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
        toast.error("Thực hiện thay đổi thất bại",
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

      console.log("reportprocessings", response.data);
    }
    catch (error) {
      console.log(error);
    }
  }

  function handleBlockAllPost() {
    fetchBlockAllPosts()
  }

  return (
    <div className="h-80">
      <div className="form-head page-titles d-flex  align-items-center">
        <div className="mr-auto  d-lg-block">
          <h2 className="text-black font-w600">Quản lý báo cáo</h2>
          <ol className="breadcrumb">
            <li className="breadcrumb-item active">
              <Link to="/order-list">Báo cáo</Link>
            </li>
            <li className="breadcrumb-item">
              Báo cáo đến hạn
            </li>
          </ol>
        </div>
        {
          reportProcessings.length > 0 ? (<Button
            className="btn btn-primary rounded light mr-3"
            onClick={handleBlockAllPost}
          >
          Khoá tất cả
        </Button>) : ""
        }
        
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
      {/* <div className="form-head page-titles d-flex  align-items-center">
        <div className="mr-auto  d-lg-block">
          <h2 className="text-black font-w600">Customer</h2>
          <ol className="breadcrumb">
            <li className="breadcrumb-item active">
              <Link to="/customer-list">Customer</Link>
            </li>
            <li className="breadcrumb-item">
              <Link to="/customer-list">Customer List</Link>
            </li>
          </ol>
        </div>
        <Button
          className="btn btn-primary rounded light mr-3"
          onClick={handleBlockAllPost}
        >
          Khoá tất cả
        </Button>
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
      </div> */}
      <div className="row">
        <div className="col-lg-12">
          <div className='table-responsive table-hover fs-14'>
            <div id='orderList' className='dataTables_wrapper no-footer'>
              
            </div>
          </div>
          {
            reportProcessings.length > 0 ? (<div className="card">
              <div className="card-body " style={{ padding: "1.25rem" }}>

                <div className="table-responsive">
                  <table className="table table-sm mb-0 table-responsive-lg text-black">
                    <thead>
                      <tr>

                        <th className="align-middle" width="200">Bài viết bị báo cáo</th>
                        <th className="align-middle pr-7" width="150">Ngày xử lý</th>
                        <th className="align-middle minw200">Tiêu đề</th>
                      </tr>
                    </thead>
                    <tbody id="orders">
                      {
                        reportProcessings.map(reportprocessing => (<tr className="btn-reveal-trigger">

                          <td className="py-2">
                            <Link to={`/ecom-product-detail/${reportprocessing.postID}`}>
                              <strong>{addLeadingZeros(reportprocessing.postID)}</strong>
                            </Link>
                          </td>
                          <td className="py-2">{formatDateTimeDDmmYYYY(reportprocessing.createdDate)}</td>
                          <td className="py-2">
                            {reportprocessing.title}
                          </td>

                        </tr>))
                      }

                    </tbody>
                  </table>
                  {
                    paggination.length > 1 && (<Nav className="pull-right">{pag(paggination.length, true, 'primary', false, false)}</Nav>)
                  }

                </div>
              </div>
            </div>) : (<Alert
              key={1}
              variant={'info'}
              className='alert-dismissible solid  fade show col-xl-12'
            >
              <svg
                viewBox='0 0 24 24'
                width='24'
                height='24'
                stroke='currentColor'
                strokeWidth='2'
                fill='none'
                strokeLinecap='round'
                strokeLinejoin='round'
                className='mr-2'
              >
                <circle cx='12' cy='12' r='10'></circle>
                <line x1='12' y1='16' x2='12' y2='12'></line>
                <line x1='12' y1='8' x2='12.01' y2='8'></line>
              </svg>
              <strong>{'Thông tin!'}</strong> {'Không có báo cáo nào tới hạn.'}
            </Alert>)
          }
          
        </div>
      </div>
    </div>
  );
};

export default ReportProcessingPost;
