/// Menu
import MetisMenu from "metismenujs";
import React, { Component, useEffect } from "react";
/// Scroll
import PerfectScrollbar from "react-perfect-scrollbar";
/// Link
import { Link } from "react-router-dom";

import { useSelector } from "react-redux";
//import icon1 from "../../../images/icon1.png";

class MM extends Component {
  componentDidMount() {
    this.$el = this.el;
    this.mm = new MetisMenu(this.$el);
  }
  componentWillUnmount() {
  }
  render() {
    return (
      <div className="mm-wrapper">
        <ul className="metismenu" ref={(el) => (this.el = el)}>
          {this.props.children}
        </ul>
      </div>
    );
  }
}

const SideBar = ({ onNote }) => {

  let path = window.location.pathname;
  path = path.split("/");
  path = path[path.length - 1];

  const userInfo = useSelector(state => state.auth.auth)
  console.log(userInfo.rolename)

  
  

  return (
    <div className="deznav">
      <PerfectScrollbar className="deznav-scroll">
        <MM className="metismenu" id="menu">
          <li className={`${path === "" ? "mm-active" : ""}`}>
            <Link className="has-arrow ai-icon" to="/"  >
              <i class="las la-chart-bar"></i>
              <span className="nav-text">Trang chủ</span>
            </Link>
          </li>
          {
            userInfo.rolename === "Admin" ? (
              <li className={`${path === "customer-list" ? "mm-active" : ""}`} >
                <Link className="has-arrow ai-icon" to="/customer-list" >
                  <i class="las la-users"></i>
                  <span className="nav-text">Quản lý người dùng</span>
                </Link>
              </li>
            ) : ""
          }
          
          <li className={`${path === "order-list" ? "mm-active" : ""}`} >
            <Link className="has-arrow ai-icon"  to="/order-list" >
              <i class="las la-newspaper"></i>
              <span className="nav-text">Quản lý bài đăng</span>
            </Link>
          </li>
          <li className={`${path === "news-list" ? "mm-active" : ""}`}  >
            <Link className="has-arrow ai-icon"  to="/news-list" >
              <i class="las la-map"></i>
              <span className="nav-text">Quản lý tin tức</span>
            </Link>
          </li>
          <li className={`${path === "report-post" ? "mm-active" : ""}`}  >
            <Link className="has-arrow ai-icon"  to="/report-post" >
              <i class="las la-balance-scale"></i>
              <span className="nav-text">Quản lý báo cáo</span>
            </Link>
          </li>
          <li className={`${path === "processing-report" ? "mm-active" : ""}`}  >
            <Link className="has-arrow ai-icon"  to="/processing-report" >
              <i class="las la-stopwatch"></i>
              <span className="nav-text">Báo cáo đến hạn</span>
            </Link>
          </li>
        </MM>

      </PerfectScrollbar>
    </div>
  )
}



// class SideBar extends Component {
//   /// Open menu
//   componentDidMount() {
//     // sidebar open/close
//     var btn = document.querySelector(".nav-control");
//     var aaa = document.querySelector("#main-wrapper");
//     function toggleFunc() {
//       return aaa.classList.toggle("menu-toggle");
//     }
//     btn.addEventListener("click", toggleFunc);
//   }
//   // state = {
//   //   loveEmoji: false,
//   // };

  

//   render() {
//     /// Path
//     let path = window.location.pathname;
//     path = path.split("/");
//     path = path[path.length - 1];


    

//     return (
//       <div className="deznav">
//         <PerfectScrollbar className="deznav-scroll">
//           <MM className="metismenu" id="menu">
//             <li className={`${path === "" ? "mm-active" : ""}`}>
//               <Link className="has-arrow ai-icon" to="/" onClick={() => this.props.onClick3()} >
//                 <i class="las la-chart-bar"></i>
//                 <span className="nav-text">Trang chủ</span>
//               </Link>
//             </li>
//             <li className={`${path === "customer-list" ? "mm-active" : ""}`} >
//               <Link className="has-arrow ai-icon" onClick={() => this.props.onClick()} to="/customer-list" >
//                 <i class="las la-users"></i>
//                 <span className="nav-text">Quản lý người dùng</span>
//               </Link>
//             </li>
//             <li className={`${path === "order-list" ? "mm-active" : ""}`} >
//               <Link className="has-arrow ai-icon" onClick={() => this.props.onClick()} to="/order-list" >
//                 <i class="las la-newspaper"></i>
//                 <span className="nav-text">Quản lý bài đăng</span>
//               </Link>
//             </li>
//             <li className={`${path === "news-list" ? "mm-active" : ""}`}  >
//               <Link className="has-arrow ai-icon" onClick={() => this.props.onClick()} to="/news-list" >
//                 <i class="las la-map"></i>
//                 <span className="nav-text">Quản lý tin tức</span>
//               </Link>
//             </li>
//             <li className={`${path === "report-post" ? "mm-active" : ""}`}  >
//               <Link className="has-arrow ai-icon" onClick={() => this.props.onClick()} to="/report-post" >
//                 <i class="las la-balance-scale"></i>
//                 <span className="nav-text">Quản lý báo cáo</span>
//               </Link>
//             </li>
//             <li className={`${path === "processing-report" ? "mm-active" : ""}`}  >
//               <Link className="has-arrow ai-icon" onClick={() => this.props.onClick()} to="/processing-report" >
//                 <i class="las la-stopwatch"></i>
//                 <span className="nav-text">Báo cáo đến hạn</span>
//               </Link>
//             </li>
//           </MM>

//         </PerfectScrollbar>
//       </div>
//     );
//   }
// }

export default SideBar;
