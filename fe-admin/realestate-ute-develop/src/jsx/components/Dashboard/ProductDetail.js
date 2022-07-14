import React, { useEffect, useState } from "react";
import { Modal, Nav, Tab } from "react-bootstrap";
import { Link, useHistory, useLocation, useParams } from "react-router-dom";
import PageTitle from "../../layouts/PageTitle";
import { useDispatch, useSelector } from 'react-redux';
import { getPostById, updateStatusPost } from '../../../services/PostsService';
import { getLocation } from '../../../services/MapService';
import { getProvincesAction } from '../../../store/actions/ProvinceActions';
import { Carousel, Button } from 'react-bootstrap';
import { Parser } from 'html-to-react';
import swal from "sweetalert";
import Map from '../AppsMenu/Shop/ProductGrid/Map';
import { confirmedUpdateStatusPostAction } from '../../../store/actions/PostActions';
import { updateStatus, updatePostType, updateLocation, addLeadingZeros, formatPrice, updatePaper, updateDirection } from "../../../util/common";
import { useRef } from "react";
import { GetReportProcessingByPost, UpdateStatusReportProcessing } from '../../../services/ReportProcessingService';
import { GetReportByPost } from '../../../services/ReportsService';
import { Pagination } from 'react-bootstrap';
import { formatDateTimeDDmmYYYYHHMM } from '../../../util/common';
import { ToastContainer, toast } from 'react-toastify';


const ProductDetail = () => {
	
	let param = useParams()
	let postId = parseInt(param.postId)

	const provinces = useSelector(state => state.provinces.provinces)
	const provinceStatus = useSelector(state => state.provinces.status)

	const dispatch = useDispatch();
	let history = useHistory();

	useEffect(() => {
		if (provinceStatus === 'idle') {
			dispatch(getProvincesAction())
		}
	}, [provinceStatus])

	const [post, setPost] = useState({})
	useEffect(() => {
		const fetchPost = async () => {
			try {
				const response = await getPostById(postId);
				setPost(response.data)
			}
			catch (error) {
				console.log(error)
			}
		}

		fetchPost()
	}, [])

	const [center, setCenter] = useState();

	useEffect(() => {
		const fetchLocation = async (address) => {
			if (address) {
				try {
					const response = await getLocation(address);
					console.log("response", response)
					if (response.data.results.length > 0) {
						setCenter(response.data.results[0].geometry.location);
					}
				}
				catch (error) {
					console.log(error)
				}
			}
		}
		if (post) {
			fetchLocation(updateLocation(provinces, post.address, post.provinceID, post.districtID, post.wardID).join(', '))
		}
	}, [post])

	async function handleAccept(postId) {
		let data = {
			id: postId,
			statusID: 2
		}

		try {
			const response = await updateStatusPost(data)
			if (response.data) {
				dispatch(confirmedUpdateStatusPostAction(data))
				setPost(response.data)
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
		catch (error) {
			console.log(error)
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

	async function handleReject(postId) {
		let data = {
			id: postId,
			statusID: 3
		}

		try {
			const response = await updateStatusPost(data)
			if (response.data) {
				dispatch(confirmedUpdateStatusPostAction(data))
				setPost(response.data)
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
		catch (error) {
			console.log(error)
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


	//----------------------------------------xử lý sau nè -------------------------------------------------------------

	const [reportStatus, setReportStatus] = useState(false);

	const [reportsProcessed, setReportsProcessed] = useState([]);
	const [totalSizeProcessed, setTotalSizeProcessed] = useState(0);

	const [reportsNoProcess, setReportsNoProcess] = useState([]);
	const [totalSizeNoProcess, setTotalSizeNoProcess] = useState(0);

	const size = 2;

	const activePagProcessed = useRef(1);
	const activePagNoProcess = useRef(1);


	let pagginationProcessed = Array(Math.ceil(totalSizeProcessed / size))
		.fill()
		.map((_, i) => i + 1)
	
	let pagginationNoProcess = Array(Math.ceil(totalSizeNoProcess / size))
		.fill()
		.map((_, i) => i + 1)

	const onClick = (i, status, activePag) => {
		activePag.current = i
		chageData(activePag.current, status)
	}



	const fetchReports = async (page, status) => {
		try {

			const response = await GetReportByPost(postId, {
				page: page,
				size: size,
				status: status
			})

			console.log("responseProductDetail", response)

			if (status) {
				setReportsProcessed(response.data.reports);
				setTotalSizeProcessed(response.data.totalSize);
			}
			else {
				setReportsNoProcess(response.data.reports);
				setTotalSizeNoProcess(response.data.totalSize);
			}

			// setReports(response.data.reports);
			// setTotalSize(response.data.totalSize);

			console.log("reports", response.data);
		}
		catch (error) {
			console.log(error);
		}
	}


	useEffect(() => {

		if (reportStatus) {
			activePagProcessed.current = 1
			fetchReports(activePagProcessed.current, reportStatus)
		}
		else {
			activePagNoProcess.current = 1
			fetchReports(activePagNoProcess.current, reportStatus)
		}	
	}, [reportStatus])














	const chageData = (page, status) => {

		fetchReports(page, status)
		
	}

	function handleReport(value) {
		setReportStatus(value)

		console.log("handleReport", value)
	}

	const pag = (size, gutter, variant, bg, circle, activePag, status, paggination) => (
		<Pagination

			className={`mt-4  ${gutter ? 'pagination-gutter' : ''} ${variant && `pagination-${variant}`
				} ${!bg && 'no-bg'} ${circle && 'pagination-circle'}`}
		>
			<li className='page-item page-indicator'>
				<Link className='page-link' to='#' onClick={() =>
					activePag.current > 0 && onClick(activePag.current - 1, status, activePag)
				}>
					<i className='la la-angle-left' />
				</Link>
			</li>
			{
				paggination.map((number, i) => (
					<Pagination.Item key={number} active={number === activePag.current} onClick={() => onClick(number, status, activePag)}>
						{number}
					</Pagination.Item>

				))
			}
			<li className='page-item page-indicator'>
				<Link className='page-link' to='#' onClick={() =>
					activePag.current < paggination.length &&
					onClick(activePag.current + 1, status, activePag)
				}>
					<i className='la la-angle-right' />
				</Link>
			</li>
		</Pagination>
	)

	const [statusProcessing, setStatusProcessing] = useState(0);

	const fetchGetReportProcessingByPost = async () => {
		try {

			const response = await GetReportProcessingByPost(postId)

			// setReports(response.data.reports);
			// setTotalSize(response.data.totalSize);

			console.log("reports----------", response.data);
			setStatusProcessing(response.data);
		}
		catch (error) {
			console.log(error);
		}
	}

	useEffect(() => {

		fetchGetReportProcessingByPost()
	}, [])


	const fetchUpdateStatusReportProcessing = async (id, statusId) => {
		try {

			console.log("id",id)
			const response = await UpdateStatusReportProcessing({ id: id, statusId: statusId })

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

				fetchGetReportProcessingByPost()
				if (reportStatus) {
					fetchReports(activePagProcessed.current, reportStatus)
				}
				else {
					fetchReports(activePagNoProcess.current, reportStatus)
				}
					
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

			// setReports(response.data.reports);
			// setTotalSize(response.data.totalSize);

			console.log("meomeo----------", response.data);
			// setStatusProcessing(response.data);
		}
		catch (error) {
			console.log(error);
		}
	}

	return (
		<>
			<div className="form-head page-titles d-flex  align-items-center">
				<div className="mr-auto  d-lg-block">
					<h2 className="text-black font-w600">Quản lý bài đăng</h2>
					<ol className="breadcrumb">
						<li className="breadcrumb-item active">
							<Link to="/order-list">Bài đăng</Link>
						</li>
						<li className="breadcrumb-item">
							Chi tiết bài đăng
						</li>
					</ol>
				</div>
			</div>
			<div className="row">
				<div className="col-lg-12">
					<div className="card">
						<div className="card-body">
							<div className="row">
								<div className="col-xl-5 col-lg-6  col-md-6 col-xxl-5 ">
									<Carousel>
										{
											post && post.imageUrls && (
												post.imageUrls.map((img, index) => (<Carousel.Item key={index}>
													<img
														src={img}
														className='d-block w-100'
														alt={`Slide ${index + 1}`}
														height={450}
													/>
												</Carousel.Item>))
											)}
									</Carousel>
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
								<div className="col-xl-7 col-lg-6  col-md-6 col-xxl-7 col-sm-12">
									<div className="product-detail-content">
										<div className="new-arrival-content pr">
											<h3>{post && post.title}</h3>
											<div className="d-table mb-2">
												<p className="price float-left d-block">{post && formatPrice(post.price)}</p>
											</div>
											<p> Tình trạng bài đăng:{" "}<span className={post && updateStatus(post.statusID).className}>{post && updateStatus(post.statusID).text}</span></p>
											<p> Tình trạng bất động sản:{" "}{post && (post.isSold ? "Đã bán" : "Chưa bán")}</p>
											<p> Mã bài đăng: {addLeadingZeros(post.id)} </p>
											<p> Danh mục: {post && post.categoryName}</p>
											<p> Loại bài đăng: {post && updatePostType(post.postTypeID)}</p>
											<p className="text-content">
												{post && Parser().parse(post.details) }
											</p>
											
											<div className="row px-0">
												{
													(post && post.bedrooms) ? (<div className="col-xl-6 col-lg-6  col-md-6 col-xxl-6 col-sm-6">
														<p> <i className="las la-check-circle" /> Số phòng ngủ: <span>{post && post.bedrooms}</span></p>
													</div>) : ""
												}
												{
													(post && post.bathrooms) ? (<div className="col-xl-6 col-lg-6  col-md-6 col-xxl-6 col-sm-6">
														<p> <i className="las la-check-circle" /> Số phòng vệ sinh: <span>{post && post.bathrooms}</span></p>
													</div>) : ""
												}
												
											</div>
											<div className="row px-0">
												{
													(post && updatePaper(post.paperID)) ? (<div className="col-xl-6 col-lg-6  col-md-6 col-xxl-6 col-sm-6">
														<p> <i className="las la-check-circle" /> Giấy tờ pháp lý: <span>{post && updatePaper(post.paperID)}</span></p>
													</div>) : ""
												}
												{
													(post && updateDirection(post.directionID)) ? (<div className="col-xl-6 col-lg-6  col-md-6 col-xxl-6 col-sm-6">
														<p> <i className="las la-check-circle" /> Hướng cửa chính: <span>{post && updateDirection(post.directionID)}</span></p>
													</div>) : ""
												}
											</div>

											<p className="text-content">
												{post && updateLocation(provinces, post.address, post.provinceID, post.districtID, post.wardID).join(', ')}
											</p>
											
											<div className="shopping-cart mt-3">
												<Button className='mr-2' variant='success'
													onClick={() =>
														swal({
															title: `Bạn có chắc chắn đồng ý bài đăng ${addLeadingZeros(post.id)}?`,
															text:
																"Once deleted, you will not be able to recover this imaginary file!",
															icon: "warning",
															buttons: true,
															dangerMode: true,
														}).then((willAccept) => {
															if (willAccept) {
																handleAccept(post.id)
															}
														})
													}
												>
													Đồng ý
												</Button>
												<Button className='mr-2' variant='primary'
													onClick={() =>
														swal({
															title: `Bạn có chắc chắn từ chối bài đăng ${addLeadingZeros(post.id)}?`,
															text:
																"Once deleted, you will not be able to recover this imaginary file!",
															icon: "warning",
															buttons: true,
															dangerMode: true,
														}).then((willReject) => {
															if (willReject) {
																handleReject(post.id)
															}
														})
													}
												>
													Từ chối
												</Button>
												<Button className='mr-2' variant='danger' onClick={() => history.goBack()}>
													Trở lại
												</Button>
											</div>
										</div>
									</div>
								</div>
							</div>
							<div className="mt-3">
								{
									center && (<Map
										location={center}
										googleMapURL={`https://maps.googleapis.com/maps/api/js?key=${"AIzaSyD0KEGkCK2HxQrnFteb79Jpp6XQw1LfqNg"}&v=3.exp&libraries=geometry,drawing,places`}
										loadingElement={<div style={{ height: `100%` }} />}
										containerElement={<div style={{ height: `90vh`, margin: `auto` }} />}
										mapElement={<div style={{ height: `100%` }}
										 />}
									/>)
								}
								
							</div>
							
							<div className="row mt-5">
								<div className="col-xl-12">
									
									<div className="row">
										<div className="col-xl-6">
											<h2 className="">BÁO CÁO VI PHẠM</h2>
										</div>
										{
											statusProcessing.statusID > 0 && (
												<div className="col-xl-6">
													<Button
														className="btn btn-danger rounded pull-right"
														onClick={() => fetchUpdateStatusReportProcessing(statusProcessing.id, 4)}
													>
														Từ chối báo cáo

													</Button>
													{
														statusProcessing.statusID === 1 && (
															<Button
																className="btn btn-danger rounded light mr-3 pull-right"
																onClick={() => fetchUpdateStatusReportProcessing(statusProcessing.id, 2)}
															>
																Chờ phản hồi
															</Button>
														)
													}
													{
														statusProcessing.statusID === 2 && (
															<Button
																className="btn btn-danger rounded light mr-3 pull-right"
																onClick={() => fetchUpdateStatusReportProcessing(statusProcessing.id, 3)}
															>
																Đồng ý báo cáo
															</Button>
														)
													}
													
													
												</div>
											)
										}
										
										

									</div>

									
									<Tab.Container defaultActiveKey="navpills-1">
										<Nav className="nav nav-pills review-tab" variant="" as="ul">
											<Nav.Item as="li">
												<Nav.Link eventKey="navpills-1" onClick={()=>handleReport(false)}>Chưa xử lý</Nav.Link>
											</Nav.Item>
											<Nav.Item as="li">
												<Nav.Link eventKey="navpills-2" onClick={() => handleReport(true)}>Đã xử lý</Nav.Link>
											</Nav.Item>
										</Nav>
										<Tab.Content className="tab-content pt-4 bg-white">
											<Tab.Pane eventKey="navpills-1">
												{
													reportsNoProcess.map((value) => (<div className="card review-table">
														<div className="media align-items-center">
															<div className="media-body d-lg-flex d-block row align-items-center">
																<div className="col-xl-2 col-xxl-3 col-lg-12 review-bx">
																	<h3 className="fs-20 text-black font-w600 mb-1">
																		<Link to={`../user-details/${value.userID}`}>{value.name}</Link>
																		{/* {value.name} */}
																	</h3>
																	<span className="d-block mb-xl-0 mb-3">
																		{formatDateTimeDDmmYYYYHHMM(value.createdDate)}
																		{/* Join on 26/04/2020, 12:42 AM */}
																	</span>
																</div>
																<div className="col-xl-10 col-xxl-9 col-lg-12 text-dark mb-xl-0 mb-2">
																	{value.details}
																</div>
															</div>
															
														</div>
													</div>))
												}
												{
													pagginationNoProcess.length > 1 && (<Nav className="pull-right">{pag(pagginationNoProcess.length, true, 'primary', false, false, activePagNoProcess, false, pagginationNoProcess)}</Nav>)
												}
												
												
											</Tab.Pane>
											<Tab.Pane eventKey="navpills-2">
												{
													reportsProcessed.map((value) => (<div className="card review-table">
														<div className="media align-items-center">
															<div className="media-body d-lg-flex d-block row align-items-center">
																<div className="col-xl-2 col-xxl-3 col-lg-12 review-bx">
																	<h3 className="fs-20 text-black font-w600 mb-1">
																		<Link to={`../user-details/${value.userID}`}>{value.name}</Link>
																		{/* {value.name} */}
																	</h3>
																	<span className="d-block mb-xl-0 mb-3">
																		{formatDateTimeDDmmYYYYHHMM(value.createdDate)}
																		{/* Join on 26/04/2020, 12:42 AM */}
																	</span>
																</div>
																<div className="col-xl-10 col-xxl-9 col-lg-12 text-dark mb-xl-0 mb-2">
																	{value.details}
																</div>
															</div>

														</div>
													</div>))
												}

												{
													pagginationProcessed.length > 1 && (<Nav className="pull-right">{pag(pagginationProcessed.length, true, 'primary', false, false, activePagProcessed, true, pagginationProcessed)}</Nav>)
												}

												
												
											</Tab.Pane>
										</Tab.Content>
									</Tab.Container>
								</div>
							</div>
						</div>
					</div>
				</div>
				
			</div>
		</>
	);
};

export default ProductDetail;
