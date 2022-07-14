import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";

//** Import Image */
import avatar from "../../../images/avatar/avatar.png";
import avatar1 from "../../../images/avatar/1.jpg";
import map from "../../../images/map.jpg";
import customers10 from "../../../images/customers/10.jpg";
import customers11 from "../../../images/customers/11.jpg";
import customers12 from "../../../images/customers/12.jpg";
import FrontViewSlider from "../Omah/PropertyDetails/FrontViewSlider";
import ImageGallery from "../Omah/PropertyDetails/ImageGallery";

import { formatDateTimeDDmmYYYY } from "../../../../src/util/common";

import { getUserById, lockunlock } from '../../../services/UsersService';
import { Row, Col, Card, Accordion, Carousel, Alert } from 'react-bootstrap'
import { Dropdown, Modal } from 'react-bootstrap';
import swal from "sweetalert";
import { useDispatch, useSelector } from 'react-redux';
import { confirmLockUnlockUserAction } from '../../../store/actions/UserActions';
import { Nav, Pagination } from 'react-bootstrap';
import { getPosts } from '../../../services/PostsService';
import { useRef } from "react";
import { addLeadingZeros, updateLocation, updateStatus, updatePostType } from '../../../util/common.js';
import { getProvincesAction } from '../../../store/actions/ProvinceActions';
import { GetReportProcessingByPost } from '../../../services/ReportProcessingService';


import card1 from './../../../images/task/img1.jpg';
import card2 from './../../../images/task/img2.jpg';
import card3 from './../../../images/task/img3.jpg';
import card4 from './../../../images/task/img4.jpg';
import card5 from './../../../images/task/img5.jpg';
import card6 from './../../../images/task/img6.jpg';
import card7 from './../../../images/task/img7.jpg';
import card8 from './../../../images/task/img8.jpg';


function UserDetails() {
    const imgs = [card1, card2, card3, card4, card5, card6, card7, card8];
    const userInfo = useSelector(state => state.auth.auth)

    function randomImage() {
        let number = Math.floor(Math.random() * imgs.length);
        return imgs[number]
    }
    const dispatch = useDispatch();
    let param = useParams();
    let userId = param.userId;
    if (!userId && userInfo) {
        userId = userInfo.id
    }

    const sort = 4;
    const activePag = useRef(1);


    const [user, setUser] = useState({});
    const [posts, setPosts] = useState([]);
    const [totalSize, setTotalSize] = useState(0);
    const [pageNumber, setPageNumber] = useState(0);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await getUserById(userId);
                console.log("user...", response)
                setUser(response.data)
            }
            catch (error) {
                console.log(error)
            }
        }

        fetchUser()
    }, []);

    const fetchPosts = async (page) => {
        try {

            const response = await getPosts({
                userID: userId,
                page: page,
                size: sort
            })

            setPosts(response.data.posts);
            setTotalSize(response.data.totalSize);
            // setPageNumber(response.data.pageNumber);
            console.log("posts", response.data);
        }
        catch (error) {
            console.log(error);
        }
    }

    useEffect(() => {

        fetchPosts(activePag.current)
    }, [])

    const provinces = useSelector(state => state.provinces.provinces)
    const provinceStatus = useSelector(state => state.provinces.status)

    useEffect(() => {
        if (provinceStatus === 'idle') {
            dispatch(getProvincesAction())
        }
    }, [provinceStatus])

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

    let paggination = Array(Math.ceil(totalSize / sort))
        .fill()
        .map((_, i) => i + 1)
    console.log("totalSize", totalSize)

    const onClick = (i) => {
        activePag.current = i
        chageData(activePag.current)
    }

    const chageData = (page) => {
        fetchPosts(page)

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
                            Thông tin người dùng
                        </li>
                    </ol>
                </div>
                <Link to={`/update-info/${userId}`} className="btn btn-danger rounded mr-3">
                    Thay đổi thông tin
                </Link>
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
                                <Card>
                                    <Card.Header className='d-block card-header'>
                                        <Card.Title>Danh sách bài đăng</Card.Title>
                                        <Card.Text className='m-0 subtitle'>
                                            Danh sách các bài đăng do người dùng tạo
                                        </Card.Text>
                                    </Card.Header>
                                    <Card.Body className='card-body'>
                                        <Accordion
                                            className='accordion accordion-primary'
                                            defaultActiveKey='0'
                                        >
                                            <div className="row" >
                                                {
                                                    posts.length == 0 ? (<Alert
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
                                                        <strong>{'Thông tin!'}</strong> {'Không có bài viết nào được tạo.'}
                                                    </Alert>) :
                                                        posts.map((post, index) => (
                                                            <div className="col-xl-6 col-xxl-6 col-lg-6 col-md-6 col-sm-6" key={index}>
                                                                <div className="card project-boxed">
                                                                    <div className="img-bx">
                                                                        <img src={post.imageUrls[0] ? post.imageUrls[0] : randomImage()} alt="" className=" mr-3 card-list-img w-100" width="130" height="240" />
                                                                    </div>

                                                                    <div className="card-header align-items-start">
                                                                        <div className="p-0">
                                                                            <p className="fs-14 mb-2 text-primary">{addLeadingZeros(post.id)}</p>
                                                                            <h6 className="fs-18 font-w500 mb-0"><Link to={`../ecom-product-detail/${post.id}`} className="text-black user-name">{post.title}</Link></h6>
                                                                            {/* <div className="text-dark fs-14 text-nowrap"><i className="fa fa-calendar-o mr-3" aria-hidden="true"></i>{formatDateTimeDDmmYYYY(post.createdDate)}</div> */}
                                                                        </div>


                                                                    </div>
                                                                    <div className="card-body p-0 pb-3">
                                                                        <ul className="list-group list-group-flush">
                                                                            <li className="list-group-item">
                                                                                <span className="mb-0 title">Tình trạng</span> :
                                                                                <span className="text-black ml-2">{updateStatus(post.statusID).text}</span>
                                                                            </li>
                                                                            <li className="list-group-item">
                                                                                <span className="mb-0 title">Loại bài đăng</span> :
                                                                                <span className="text-black ml-2">{updatePostType(post.postTypeID)}</span>
                                                                            </li>
                                                                            <li className="list-group-item">
                                                                                <span className="mb-0 title">Danh mục</span> :
                                                                                <span className="text-black ml-2">{post.categoryName}</span>
                                                                            </li>
                                                                            <li className="list-group-item">
                                                                                <span className="mb-0 title">Địa điểm</span> :
                                                                                <span className="text-black desc-text ml-2">{updateLocation(provinces, post.address, post.provinceID, post.districtID, post.wardID).join(', ')}</span>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        ))
                                                }

                                            </div>
                                            {
                                                paggination.length > 1 && (<Nav className="pull-right">{pag(paggination.length, true, 'primary', false, false)}</Nav>)
                                            }

                                        </Accordion>
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

export default UserDetails;
