import React, { useEffect, useRef, useState } from "react";

import PageTitle from "../../layouts/PageTitle";

import { Dropdown } from "react-bootstrap";
import { Link, useLocation } from "react-router-dom";
import { getReports } from "../../../services/ReportsService";
import { addLeadingZeros, formatDateTimeDDmmYYYY } from "../../../util/common";
import { Nav, Pagination } from 'react-bootstrap';
import Select from "@material-ui/core/Select";
import MenuItem from '@material-ui/core/MenuItem';

const displayStatus = [
  { id: true, text: "Đã xử lý" },
  { id: false, text: "Chưa xử lý" }
]

const ReportPost = () => {
  

  const motherChackBox = document.querySelector(".product_order_single");
  const chackbox = document.querySelectorAll(".product_order");
  const chackboxFun = (type) => {
    for (let i = 0; i < chackbox.length; i++) {
      const element = chackbox[i];
      if (type === "all") {
        if (motherChackBox.checked) {
          element.checked = true;
        } else {
          element.checked = false;
        }
      } else {
        if (!element.checked) {
          motherChackBox.checked = false;
          break;
        } else {
          motherChackBox.checked = true;
        }
      }
    }
  };


  //----------------------------------------------------------------------------------------------------------
  const [reports, setReports] = useState([]);
  const [totalSize, setTotalSize] = useState(0);

  const size = 4;
  const activePag = useRef(1);

  const fetchReports = async (data) => {
    try {

      const response = await getReports(data)

      setReports(response.data.reports);
      setTotalSize(response.data.totalSize);

      console.log("reports", response.data);
    }
    catch (error) {
      console.log(error);
    }
  }


  useEffect(() => {
    const data = {
      page: activePag.current,
      size: size
    }

    fetchReports(data)
  }, [])

  let paggination = Array(Math.ceil(totalSize / size))
    .fill()
    .map((_, i) => i + 1)
  
  const onClick = (i) => {
    activePag.current = i
    handleSubmit()
  }

  function handleSubmit() {
    let data = {
      page: activePag.current,
      size: size
    }
    if (status.current !== 'default')
      data.status = status.current //cần xem lại ở đây tham số là gì

    fetchReports(data)
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

  const status = useRef('default');

  function handleSelectProcess(event) {
    status.current = event.target.value
    activePag.current = 1

    handleSubmit()
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
              Danh sách báo cáo
            </li>
          </ol>
        </div>
      </div>
      <div className="row">

        <div className="col-xl-3">
          <Select defaultValue="default" name="role" fullWidth variant="outlined" style={{ color: '#7e7e7e', backgroundColor: '#fff', padding: '0', height: '49px' }} onChange={handleSelectProcess}>
            <MenuItem value="default">
              {"Tất cả trạng thái"}
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
      <div className="row">
        <div className="col-lg-12">
          <div className='table-responsive table-hover fs-14 mt-3'>
            <div id='orderList' className='dataTables_wrapper no-footer'>
              
            </div>
          </div>
          <div className="card">
            <div className="card-body " style={{ padding: "1.25rem" }}>
              
              <div className="table-responsive">
                <table className="table table-sm mb-0 table-responsive-lg text-black">
                  <thead>
                    <tr>
                      
                      <th className="align-middle" width="300">Bài viết bị báo cáo</th>
                      <th className="align-middle pr-7" width="150">Ngày báo cáo</th>
                      <th className="align-middle minw200">Nội dung</th>
                    </tr>
                  </thead>
                  <tbody id="orders">
                    {
                      reports.map(report => (<tr className="btn-reveal-trigger">

                        <td className="py-2">
                          <Link to={`/ecom-product-detail/${report.postID}`}>
                            <strong>{addLeadingZeros(report.postID)}</strong>
                          </Link>{" "}
                          bởi <Link to={`user-details/${report.userID}`}><strong>{ report.name }</strong></Link>
                          <br />
                          <a href="mailto:ricky@example.com">{ report.email }</a>
                        </td>
                        <td className="py-2">{formatDateTimeDDmmYYYY(report.createdDate) }</td>
                        <td className="py-2">
                          {report.details}
                        </td>
                        
                      </tr>))
                    }
                    
                  </tbody>
                </table>
                { paggination.length > 1 && (<Nav className="pull-right">{pag(paggination.length, true, 'primary', false, false)}</Nav>)}
                
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ReportPost;
