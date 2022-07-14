import React, { useState } from "react";
import { Link } from "react-router-dom";
import { Dropdown } from "react-bootstrap";

// Map
import World from "@svg-maps/world";
import { SVGMap } from "react-svg-map";
import { useDispatch, useSelector } from 'react-redux';

//** Import Image */
import customers1 from "../../../images/customers/1.jpg";
import customers2 from "../../../images/customers/2.jpg";
import customers3 from "../../../images/customers/3.jpg";

import PropertySlider from "../Omah/Home/Slider/PropertySlider";

import { report4, report1 } from '../../../services/PostsService';
import { useEffect } from 'react';
import Select from "@material-ui/core/Select";
import MenuItem from '@material-ui/core/MenuItem';
import { getProvincesAction } from '../../../store/actions/ProvinceActions'
import ChartBar from "../Omah/Home/Chart/ChartBar";

import MessengerCustomerChat from 'react-messenger-customer-chat';
import { FacebookProvider, Page } from 'react-facebook';

// Chart
import ChartDoughnut from "../Omah/Home/Chart/DonutChart";
// Apex Chart
import loadable from "@loadable/component";
import pMinDelay from "p-min-delay";
const PieChart = loadable(() =>
  pMinDelay(import("../Omah/Home/Chart/PieChart"), 500)
);
const ChartTimeLine = loadable(() =>
  pMinDelay(import("../Omah/Home/Chart/ChartTimeline"), 500)
);
// const ChartBar = loadable(() =>
//   pMinDelay(import("../Omah/Home/Chart/ChartBar"), 500)
// );

let date = new Date();
let month = date.getMonth() + 1
let months = []
for (let i = 1; i <= month; i++) {
  months.push(`Tháng ${i}`);
}
console.log(months)

function Home() {

  const [province, setProvince] = useState(79);

  const dispatch = useDispatch();
  const provinces = useSelector(state => state.provinces.provinces)
  const provinceStatus = useSelector(state => state.provinces.status)

  useEffect(() => {
    if (provinceStatus === 'idle') {
      dispatch(getProvincesAction())
    }
  }, [provinceStatus])

  function handleSelectProvince(event) {
    let province = provinces.find(province => province.code === event.target.value);
    console.log(province)
    setProvince(province.code);
    // fetchReport4(province.code)

  }




  const [housePrice, setHousePrice] = useState([]);
  const [landPrice, setLandPrice] = useState([]);
  const [officePrice, setOfficePrice] = useState([]);
  const [appartmentPrice, setAppartmentPrice] = useState([]);
  const [avgLandPrice, setAvgLandPrice] = useState();
  const [avgApartmentPrice, setAvgApartmentPrice] = useState();
  const [avgHousePrice, setAvgHousePrice] = useState();
  const [avgOfficePrice, setAvgOfficePrice] = useState();

  const [newUser, setNewUser] = useState(0);
  const [totalPosts, setTotalPosts] = useState(0);
  const [totalPostNews, setTotalPostNews] = useState(0);



  const fetchReport4 = async (id) => {
    try {

      const response = await report4(id)
      console.log("report4", response.data);

      if (response.data) {
        console.log("meomeomeo")
        setHousePrice(response.data.housePrice);
        setLandPrice(response.data.landPrice);
        setOfficePrice(response.data.officePrice);
        setAppartmentPrice(response.data.appartmentPrice);
        setAvgLandPrice(response.data.avgLandPrice);
        setAvgApartmentPrice(response.data.avgApartmentPrice);
        setAvgHousePrice(response.data.avgHousePrice);
        setAvgOfficePrice(response.data.avgOfficePrice);
      }

    }
    catch (error) {
      console.log(error);
    }
  }


  const fetchReport1 = async () => {
    try {

      const response = await report1()
      console.log("report1", response.data);

      if (response.data) {
        console.log("meomeomeo1")
        setNewUser(response.data.newUser);
        setTotalPosts(response.data.totalPosts);
        setTotalPostNews(response.data.totalPostNews);
      }

    }
    catch (error) {
      console.log(error);
    }
  }

  useEffect(() => {

    fetchReport4(province)
  }, [province]);

  useEffect(() => {

    fetchReport1()
  }, []);


  //-------------------


  // // Configs
  // let liveChatBaseUrl = document.location.protocol + '//' + 'livechat.fpt.ai/v36/src'
  // let LiveChatSocketUrl = 'livechat.fpt.ai:443'
  // let FptAppCode = 'dbab464f90681ee8f447571dfaaafdcf'
  // let FptAppName = 'Hỗ trợ trực tuyến'
  // // Define custom styles
  // let CustomStyles = {
  //   headerBackground: 'linear-gradient(86.7deg, #3353a2ff 0.85%, #31b7b7ff 98.94%)',
  //   headerTextColor: '#ffffffff',
  //   headerLogoEnable: false,
  //   headerLogoLink: 'https://chatbot-tools.fpt.ai/livechat-builder/img/Icon-fpt-ai.png',
  //   headerText: 'Hỗ trợ trực tuyến',
  //   primaryColor: '#6d9ccbff',
  //   secondaryColor: '#ecececff',
  //   primaryTextColor: '#ffffffff',
  //   secondaryTextColor: '#000000DE',
  //   buttonColor: '#b4b4b4ff',
  //   buttonTextColor: '#ffffffff',
  //   bodyBackgroundEnable: '',
  //   bodyBackgroundLink: ',',
  //   avatarBot: 'https://chatbot-tools.fpt.ai/livechat-builder/img/bot.png',
  //   sendMessagePlaceholder: 'Nhập tin nhắn',
  //   floatButtonLogo: 'https://chatbot-tools.fpt.ai/livechat-builder/img/Icon-fpt-ai.png',
  //   floatButtonTooltip: 'FPT.AI xin chào',
  //   floatButtonTooltipEnable: true,
  //   customerLogo: 'https://chatbot-tools.fpt.ai/livechat-builder/img/bot.png',
  //   customerWelcomeText: 'Vui lòng nhập tên của bạn',
  //   customerButtonText: 'Bắt đầu',
  //   prefixEnable: false,
  //   prefixType: 'radio',
  //   prefixOptions: ["Anh", "Chị"],
  //   prefixPlaceholder: 'Danh xưng',
  //   css: ''
  // }
  // // Get bot code from url if FptAppCode is empty
  // if (!FptAppCode) {
  //   let appCodeFromHash = window.location.hash.substr(1)
  //   if (appCodeFromHash.length === 32) {
  //     FptAppCode = appCodeFromHash
  //   }
  // }
  // // Set Configs
  // let FptLiveChatConfigs = {
  //   appName: FptAppName,
  //   appCode: FptAppCode,
  //   themes: '',
  //   styles: CustomStyles
  // }
  // // Append Script
  // let FptLiveChatScript = document.createElement('script')
  // FptLiveChatScript.id = 'fpt_ai_livechat_script'
  // FptLiveChatScript.src = liveChatBaseUrl + '/static/fptai-livechat.js'
  // document.body.appendChild(FptLiveChatScript)
  // // Append Stylesheet
  // let FptLiveChatStyles = document.createElement('link')
  // FptLiveChatStyles.id = 'fpt_ai_livechat_script'
  // FptLiveChatStyles.rel = 'stylesheet'
  // FptLiveChatStyles.href = liveChatBaseUrl + '/static/fptai-livechat.css'
  // document.body.appendChild(FptLiveChatStyles)
  // // Init
  // FptLiveChatScript.onload = function () {
  //   fpt_ai_render_chatbox(FptLiveChatConfigs, liveChatBaseUrl, LiveChatSocketUrl)
  // }





  return (
    <>
      <div className="form-head d-md-flex mb-sm-4 mb-3 align-items-start">
        <div className="mr-auto  d-lg-block">
          <h2 className="text-black font-w600">Trang chủ</h2>
          <p className="mb-0">Thống kê báo cáo</p>
        </div>
      </div>
      {/* <FacebookProvider appId="5429323507132483">
        <Page href="https://www.facebook.com/Atlanta-B%E1%BA%A5t-%C4%90%E1%BB%99ng-S%E1%BA%A3n-108923665216134" tabs="timeline" />
        
      </FacebookProvider> */}
      <MessengerCustomerChat
        pageId="108923665216134"
        appId="5429323507132483"
      />

      {/* <iframe src="livechat.fpt.ai/v36/src/index.html?botcode=dbab464f90681ee8f447571dfaaafdcf&botname=H%E1%BB%97%20tr%E1%BB%A3%20tr%E1%BB%B1c%20tuy%E1%BA%BFn&sendername=&scendpoint=livechat.fpt.ai%3A443&time=1657428097370&subchannel=&themes=&styles=%7B%22headerColorType%22%3A%22gradient%22%2C%22headerSolid%22%3A%22%23ededf2ff%22%2C%22headerGradient1%22%3A%22%233353a2ff%22%2C%22headerGradient2%22%3A%22%2331b7b7ff%22%2C%22headerTextColor%22%3A%22%23ffffffff%22%2C%22headerLogoEnable%22%3Afalse%2C%22headerLogoLink%22%3A%22https%3A%2F%2Fchatbot-tools.fpt.ai%2Flivechat-builder%2Fimg%2FIcon-fpt-ai.png%22%2C%22headerText%22%3A%22H%E1%BB%97%20tr%E1%BB%A3%20tr%E1%BB%B1c%20tuy%E1%BA%BFn%22%2C%22primaryColor%22%3A%22%236d9ccbff%22%2C%22secondaryColor%22%3A%22%23ecececff%22%2C%22primaryTextColor%22%3A%22%23ffffffff%22%2C%22secondaryTextColor%22%3A%22%23000000DE%22%2C%22buttonColor%22%3A%22%23b4b4b4ff%22%2C%22buttonTextColor%22%3A%22%23ffffffff%22%2C%22avatarBot%22%3A%22https%3A%2F%2Fchatbot-tools.fpt.ai%2Flivechat-builder%2Fimg%2Fbot.png%22%2C%22sendMessagePlaceholder%22%3A%22Nh%E1%BA%ADp%20tin%20nh%E1%BA%AFn%22%2C%22floatButtonLogo%22%3A%22https%3A%2F%2Fchatbot-tools.fpt.ai%2Flivechat-builder%2Fimg%2FIcon-fpt-ai.png%22%2C%22floatButtonTooltip%22%3A%22FPT.AI%20xin%20ch%C3%A0o%22%2C%22floatButtonTooltipEnable%22%3Atrue%2C%22customerLogo%22%3A%22https%3A%2F%2Fchatbot-tools.fpt.ai%2Flivechat-builder%2Fimg%2Fbot.png%22%2C%22customerWelcomeText%22%3A%22Vui%20l%C3%B2ng%20nh%E1%BA%ADp%20t%C3%AAn%20c%E1%BB%A7a%20b%E1%BA%A1n%22%2C%22customerButtonText%22%3A%22B%E1%BA%AFt%20%C4%91%E1%BA%A7u%22%2C%22prefixEnable%22%3Afalse%2C%22prefixType%22%3A%22radio%22%2C%22prefixOptions%22%3A%5B%22Anh%22%2C%22Ch%E1%BB%8B%22%5D%2C%22prefixPlaceholder%22%3A%22Danh%20x%C6%B0ng%22%2C%22headerBackground%22%3A%22linear-gradient(86.7deg%2C%20%233353a2ff%200.85%25%2C%20%2331b7b7ff%2098.94%25)%22%2C%22css%22%3A%22%22%7D"></iframe> */}

      <div className="row">
        <div className="col-xl-6 col-xxl-12">
          <div className="row">
            {/* <div className="col-xl-12">
              <div className="card bg-danger property-bx text-white">
                <div className="card-body">
                  <div className="media d-sm-flex d-block align-items-center">
                    <span className="mr-4 d-block mb-sm-0 mb-3">
                      <svg
                        width={80}
                        height={80}
                        viewBox="0 0 80 80"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          d="M31.8333 79.1667H4.16659C2.33325 79.1667 0.833252 77.6667 0.833252 75.8333V29.8333C0.833252 29 1.16659 28 1.83325 27.5L29.4999 1.66667C30.4999 0.833332 31.8333 0.499999 32.9999 0.999999C34.3333 1.66667 34.9999 2.83333 34.9999 4.16667V76C34.9999 77.6667 33.4999 79.1667 31.8333 79.1667ZM7.33325 72.6667H28.4999V11.6667L7.33325 31.3333V72.6667Z"
                          fill="white"
                        />
                        <path
                          d="M75.8333 79.1667H31.6666C29.8333 79.1667 28.3333 77.6667 28.3333 75.8334V36.6667C28.3333 34.8334 29.8333 33.3334 31.6666 33.3334H75.8333C77.6666 33.3334 79.1666 34.8334 79.1666 36.6667V76C79.1666 77.6667 77.6666 79.1667 75.8333 79.1667ZM34.9999 72.6667H72.6666V39.8334H34.9999V72.6667Z"
                          fill="white"
                        />
                        <path
                          d="M60.1665 79.1667H47.3332C45.4999 79.1667 43.9999 77.6667 43.9999 75.8334V55.5C43.9999 53.6667 45.4999 52.1667 47.3332 52.1667H60.1665C61.9999 52.1667 63.4999 53.6667 63.4999 55.5V75.8334C63.4999 77.6667 61.9999 79.1667 60.1665 79.1667ZM50.6665 72.6667H56.9999V58.8334H50.6665V72.6667Z"
                          fill="white"
                        />
                      </svg>
                    </span>
                    <div className="media-body mb-sm-0 mb-3 mr-5">
                      <h4 className="fs-22 text-white">Total Properties</h4>
                      <div className="progress mt-3 mb-2" style={{ height: 8 }}>
                        <div
                          className="progress-bar bg-white progress-animated"
                          style={{ width: "86%", height: 8 }}
                          role="progressbar"
                        >
                          <span className="sr-only">86% Complete</span>
                        </div>
                      </div>
                      <span className="fs-14">
                        431 more to break last month record
                      </span>
                    </div>
                    <span className="fs-46 font-w500">4,562</span>
                  </div>
                </div>
              </div>
            </div> */}
            <div className="col-sm-12 col-md-4">
              <div className="card">
                <div className="card-body">
                  <div className="media align-items-center">
                    <div className="media-body mr-3">
                      <h2 className="fs-36 text-black font-w600">{totalPostNews}</h2>
                      <p className="fs-18 mb-0 text-black font-w500">
                        
                        Số bài đăng chưa được duyệt
                      </p>
                      {/* <span className="fs-13">Target 3k/month</span> */}
                    </div>
                    <div className="d-inline-block position-relative donut-chart-sale">
                      {/* <ChartDoughnut value={71} backgroundColor="#3C4CB8" />
                      <small className="text-primary">71%</small>
                      <span className="circle bgl-primary" /> */}
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="col-sm-12 col-md-4">
              <div className="card">
                <div className="card-body">
                  <div className="media align-items-center">
                    <div className="media-body mr-3">
                      <h2 className="fs-36 text-black font-w600">{totalPosts}</h2>
                      <p className="fs-18 mb-0 text-black font-w500">
                        Tổng số bài đăng trong tháng
                      </p>
                      {/* <span className="fs-13">Target 3k/month</span> */}
                    </div>
                    <div className="d-inline-block position-relative donut-chart-sale">
                      {/* <ChartDoughnut value={71} backgroundColor="#3C4CB8" />
                      <small className="text-primary">71%</small>
                      <span className="circle bgl-primary" /> */}
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="col-sm-12 col-md-4">
              <div className="card">
                <div className="card-body">
                  <div className="media align-items-center">
                    <div className="media-body mr-3">
                      <h2 className="fs-36 text-black font-w600">{newUser}</h2>
                      <p className="fs-18 mb-0 text-black font-w500">
                        Số người dùng mới
                      </p>
                      {/* <span className="fs-13">Target 3k/month</span> */}
                    </div>
                    {/* <div className="d-inline-block position-relative donut-chart-sale">
                      <ChartDoughnut value={90} backgroundColor="#37D15A" />

                      <small className="text-success">90%</small>
                      <span className="circle bgl-success" />
                    </div> */}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-6 col-xxl-12">
          {/* <div className="card">
            <div className="card-header border-0 pb-0">
              <h3 className="fs-20 text-black">Total Revenue</h3>
              <Dropdown className="dropdown ml-auto">
                <Dropdown.Toggle
                  className="btn-link   i-false p-0"
                  data-toggle="dropdown"
                  variant=""
                >
                  <svg
                    width="24px"
                    height="24px"
                    viewBox="0 0 24 24"
                    version="1.1"
                  >
                    <g
                      stroke="none"
                      strokeWidth={1}
                      fill="none"
                      fillRule="evenodd"
                    >
                      <rect x={0} y={0} width={24} height={24} />
                      <circle fill="#000000" cx={5} cy={12} r={2} />
                      <circle fill="#000000" cx={12} cy={12} r={2} />
                      <circle fill="#000000" cx={19} cy={12} r={2} />
                    </g>
                  </svg>
                </Dropdown.Toggle>
                <Dropdown.Menu className="dropdown-menu dropdown-menu-right">
                  <Dropdown.Item className="dropdown-item" to="/">
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item className="dropdown-item" to="/">
                    Delete
                  </Dropdown.Item>
                </Dropdown.Menu>
              </Dropdown>
            </div>
            <div className="card-body pt-2 pb-0">
              <div className="d-flex flex-wrap align-items-center">
                <span className="fs-36 text-black font-w600 mr-3">
                  $678,345
                </span>
                <p className="mr-sm-auto mr-3 mb-sm-0 mb-3">
                  last month $563,443
                </p>
                <div className="d-flex align-items-center">
                  <svg
                    className="mr-3"
                    width={87}
                    height={47}
                    viewBox="0 0 87 47"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M29.8043 20.9254C15.2735 14.3873 5.88029 27.282 3 34.5466V46.2406H85V4.58005C70.8925 -0.868404 70.5398 8.66639 60.8409 19.5633C51.1419 30.4602 47.9677 29.0981 29.8043 20.9254Z"
                      fill="url(#paint0_linear)"
                    />
                    <path
                      d="M3 35.2468C5.88029 27.9822 15.2735 15.0875 29.8043 21.6257C47.9677 29.7984 51.1419 31.1605 60.8409 20.2636C70.5398 9.36665 70.8925 -0.168147 85 5.28031"
                      stroke="#37D159"
                      strokeWidth={6}
                    />
                    <defs>
                      <linearGradient
                        id="paint0_linear"
                        x1={44}
                        y1="-36.4332"
                        x2={44}
                        y2="45.9686"
                        gradientUnits="userSpaceOnUse"
                      >
                        <stop stopColor="#37D159" />
                        <stop offset={1} stopColor="#37D159" stopOpacity={0} />
                      </linearGradient>
                    </defs>
                  </svg>
                  <span className="fs-22 text-success mr-2">7%</span>
                  <svg
                    width={12}
                    height={6}
                    viewBox="0 0 12 6"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path d="M0 6L6 2.62268e-07L12 6" fill="#37D159" />
                  </svg>
                </div>
              </div>
              <ChartTimeLine />
            </div>
          </div> */}
        </div>
        <div className="col-xl-12 col-xxl-12">
          <div className="row">
            <div className="col-xl-8 col-xxl-12">
              <div className="card">
                <div className="card-header border-0 pb-0">
                  <h3 className="fs-20 text-black">Thống kê giá bán bất động sản theo tỉnh thành (triệu/m2)</h3>

                  <Dropdown className="dropdown ml-auto">
                    <Dropdown.Toggle
                      variant=""
                      className="btn-link   i-false p-0"
                      data-toggle="dropdown"
                    >
                      <svg
                        width="24px"
                        height="24px"
                        viewBox="0 0 24 24"
                        version="1.1"
                      >
                        <g
                          stroke="none"
                          strokeWidth={1}
                          fill="none"
                          fillRule="evenodd"
                        >
                          <rect x={0} y={0} width={24} height={24} />
                          <circle fill="#000000" cx={5} cy={12} r={2} />
                          <circle fill="#000000" cx={12} cy={12} r={2} />
                          <circle fill="#000000" cx={19} cy={12} r={2} />
                        </g>
                      </svg>
                    </Dropdown.Toggle>
                    <Dropdown.Menu className="dropdown-menu dropdown-menu-right">
                      <Dropdown.Item className="dropdown-item" to="/">
                        Edit
                      </Dropdown.Item>
                      <Dropdown.Item className="dropdown-item" to="/">
                        Delete
                      </Dropdown.Item>
                    </Dropdown.Menu>
                  </Dropdown>
                </div>
                <div className="card-body">
                  <div className="row mb-5">
                    <div className="col-xl-5">
                      <Select defaultValue="default" value={79} name="province" fullWidth variant="outlined" style={{ color: '#7e7e7e', backgroundColor: '#fff', padding: '0', height: '49px' }} onChange={handleSelectProvince}>

                        {
                          provinces.map((province) => (
                            <MenuItem key={province.code} value={province.code}>
                              {province.name}
                            </MenuItem>
                          ))
                        }
                      </Select>

                    </div>
                  </div>
                  <div className="d-sm-flex flex-wrap  justify-content-around">
                    <div className="d-flex mb-3 align-items-center">
                      <span className="rounded mr-3 bg-primary p-3">
                        <svg
                          width={26}
                          height={26}
                          viewBox="0 0 26 26"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M10.3458 25.7292H1.35412C0.758283 25.7292 0.270782 25.2417 0.270782 24.6458V9.69583C0.270782 9.42499 0.379116 9.09999 0.595783 8.93749L9.58745 0.541659C9.91245 0.270825 10.3458 0.162492 10.725 0.324992C11.1583 0.541659 11.375 0.920825 11.375 1.35416V24.7C11.375 25.2417 10.8875 25.7292 10.3458 25.7292ZM2.38328 23.6167H9.26245V3.79166L2.38328 10.1833V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M24.6458 25.7292H10.2916C9.69578 25.7292 9.20828 25.2417 9.20828 24.6458V11.9167C9.20828 11.3208 9.69578 10.8333 10.2916 10.8333H24.6458C25.2416 10.8333 25.7291 11.3208 25.7291 11.9167V24.7C25.7291 25.2417 25.2416 25.7292 24.6458 25.7292ZM11.375 23.6167H23.6166V12.9458H11.375V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M19.5541 25.7292H15.3833C14.7874 25.7292 14.2999 25.2417 14.2999 24.6458V18.0375C14.2999 17.4417 14.7874 16.9542 15.3833 16.9542H19.5541C20.1499 16.9542 20.6374 17.4417 20.6374 18.0375V24.6458C20.6374 25.2417 20.1499 25.7292 19.5541 25.7292ZM16.4666 23.6167H18.5249V19.1208H16.4666V23.6167Z"
                            fill="white"
                          />
                        </svg>
                      </span>
                      <div>
                        <p className="fs-14 mb-1">Nhà ở</p>
                        <span className="fs-18 text-black font-w700">
                          {avgHousePrice} triệu
                        </span>
                      </div>
                    </div>
                    <div className="d-flex mb-3 align-items-center">
                      <span className="rounded mr-3 bg-success p-3">
                        <svg
                          width={26}
                          height={26}
                          viewBox="0 0 26 26"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M10.3458 25.7292H1.35412C0.758283 25.7292 0.270782 25.2417 0.270782 24.6458V9.69583C0.270782 9.42499 0.379116 9.09999 0.595783 8.93749L9.58745 0.541659C9.91245 0.270825 10.3458 0.162492 10.725 0.324992C11.1583 0.541659 11.375 0.920825 11.375 1.35416V24.7C11.375 25.2417 10.8875 25.7292 10.3458 25.7292ZM2.38328 23.6167H9.26245V3.79166L2.38328 10.1833V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M24.6458 25.7292H10.2916C9.69578 25.7292 9.20828 25.2417 9.20828 24.6458V11.9167C9.20828 11.3208 9.69578 10.8333 10.2916 10.8333H24.6458C25.2416 10.8333 25.7291 11.3208 25.7291 11.9167V24.7C25.7291 25.2417 25.2416 25.7292 24.6458 25.7292ZM11.375 23.6167H23.6166V12.9458H11.375V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M19.5541 25.7292H15.3833C14.7874 25.7292 14.2999 25.2417 14.2999 24.6458V18.0375C14.2999 17.4417 14.7874 16.9542 15.3833 16.9542H19.5541C20.1499 16.9542 20.6374 17.4417 20.6374 18.0375V24.6458C20.6374 25.2417 20.1499 25.7292 19.5541 25.7292ZM16.4666 23.6167H18.5249V19.1208H16.4666V23.6167Z"
                            fill="white"
                          />
                        </svg>
                      </span>
                      <div>
                        <p className="fs-14 mb-1">Đất</p>
                        <span className="fs-18 text-black font-w700">
                          {avgLandPrice} triệu
                        </span>
                      </div>
                    </div>
                    <div className="d-flex mb-3 align-items-center">
                      <span className="rounded mr-3 bg-info p-3">
                        <svg
                          width={26}
                          height={26}
                          viewBox="0 0 26 26"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M10.3458 25.7292H1.35412C0.758283 25.7292 0.270782 25.2417 0.270782 24.6458V9.69583C0.270782 9.42499 0.379116 9.09999 0.595783 8.93749L9.58745 0.541659C9.91245 0.270825 10.3458 0.162492 10.725 0.324992C11.1583 0.541659 11.375 0.920825 11.375 1.35416V24.7C11.375 25.2417 10.8875 25.7292 10.3458 25.7292ZM2.38328 23.6167H9.26245V3.79166L2.38328 10.1833V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M24.6458 25.7292H10.2916C9.69578 25.7292 9.20828 25.2417 9.20828 24.6458V11.9167C9.20828 11.3208 9.69578 10.8333 10.2916 10.8333H24.6458C25.2416 10.8333 25.7291 11.3208 25.7291 11.9167V24.7C25.7291 25.2417 25.2416 25.7292 24.6458 25.7292ZM11.375 23.6167H23.6166V12.9458H11.375V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M19.5541 25.7292H15.3833C14.7874 25.7292 14.2999 25.2417 14.2999 24.6458V18.0375C14.2999 17.4417 14.7874 16.9542 15.3833 16.9542H19.5541C20.1499 16.9542 20.6374 17.4417 20.6374 18.0375V24.6458C20.6374 25.2417 20.1499 25.7292 19.5541 25.7292ZM16.4666 23.6167H18.5249V19.1208H16.4666V23.6167Z"
                            fill="white"
                          />
                        </svg>
                      </span>
                      <div>
                        <p className="fs-14 mb-1">Căn hộ/ Chung cư</p>
                        <span className="fs-18 text-black font-w700">
                          {avgApartmentPrice} triệu
                        </span>
                      </div>
                    </div>
                    <div className="d-flex mb-3 align-items-center">
                      <span className="rounded mr-3 bg-warning p-3">
                        <svg
                          width={26}
                          height={26}
                          viewBox="0 0 26 26"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M10.3458 25.7292H1.35412C0.758283 25.7292 0.270782 25.2417 0.270782 24.6458V9.69583C0.270782 9.42499 0.379116 9.09999 0.595783 8.93749L9.58745 0.541659C9.91245 0.270825 10.3458 0.162492 10.725 0.324992C11.1583 0.541659 11.375 0.920825 11.375 1.35416V24.7C11.375 25.2417 10.8875 25.7292 10.3458 25.7292ZM2.38328 23.6167H9.26245V3.79166L2.38328 10.1833V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M24.6458 25.7292H10.2916C9.69578 25.7292 9.20828 25.2417 9.20828 24.6458V11.9167C9.20828 11.3208 9.69578 10.8333 10.2916 10.8333H24.6458C25.2416 10.8333 25.7291 11.3208 25.7291 11.9167V24.7C25.7291 25.2417 25.2416 25.7292 24.6458 25.7292ZM11.375 23.6167H23.6166V12.9458H11.375V23.6167Z"
                            fill="white"
                          />
                          <path
                            d="M19.5541 25.7292H15.3833C14.7874 25.7292 14.2999 25.2417 14.2999 24.6458V18.0375C14.2999 17.4417 14.7874 16.9542 15.3833 16.9542H19.5541C20.1499 16.9542 20.6374 17.4417 20.6374 18.0375V24.6458C20.6374 25.2417 20.1499 25.7292 19.5541 25.7292ZM16.4666 23.6167H18.5249V19.1208H16.4666V23.6167Z"
                            fill="white"
                          />
                        </svg>
                      </span>
                      <div>
                        <p className="fs-14 mb-1">Văn phòng</p>
                        <span className="fs-18 text-black font-w700">
                          {avgOfficePrice} triệu
                        </span>
                      </div>
                    </div>
                  </div>

                  <ChartBar months={months} housePrice={housePrice} landPrice={landPrice} officePrice={officePrice} appartmentPrice={appartmentPrice} />
                </div>
              </div>
            </div>
            {/* <div className="col-xl-4 col-xxl-12">
              <div className="row">
                <div className="col-xl-12 col-xxl-6 col-md-6">
                  <div className="card">
                    <div className="card-body">
                      <div id="monocromeChart" />
                      <PieChart />
                      <div className="d-flex flex-wrap mt-3">
                        <span className="text-black font-w600 mr-5 mb-2">
                          <svg
                            className="mr-2"
                            width={20}
                            height={20}
                            viewBox="0 0 20 20"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <rect
                              width={20}
                              height={20}
                              rx={8}
                              fill="#FFB067"
                            />
                          </svg>
                          Agent
                        </span>
                        <span className="text-black font-w600 mb-2">
                          <svg
                            className="mr-2"
                            width={20}
                            height={20}
                            viewBox="0 0 20 20"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <rect
                              width={20}
                              height={20}
                              rx={8}
                              fill="#B655E4"
                            />
                          </svg>
                          Customers
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="col-xl-12 col-xxl-6 col-md-6">
                  <div className="card">
                    <div className="card-body">
                      <p className="mb-2 d-flex  fs-16 text-black font-w500">
                        Product Viewed
                        <span className="pull-right ml-auto text-dark fs-14">
                          561/days
                        </span>
                      </p>
                      <div className="progress mb-4" style={{ height: 10 }}>
                        <div
                          className="progress-bar bg-primary progress-animated"
                          style={{ width: "75%", height: 10 }}
                          role="progressbar"
                        >
                          <span className="sr-only">75% Complete</span>
                        </div>
                      </div>
                      <p className="mb-2 d-flex  fs-16 text-black font-w500">
                        Product Listed
                        <span className="pull-right ml-auto text-dark fs-14">
                          3,456 Unit
                        </span>
                      </p>
                      <div className="progress mb-3" style={{ height: 10 }}>
                        <div
                          className="progress-bar bg-primary progress-animated"
                          style={{ width: "90%", height: 10 }}
                          role="progressbar"
                        >
                          <span className="sr-only">90% Complete</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div> */}
          </div>
        </div>

      </div>
    </>
  );
}

export default Home;
