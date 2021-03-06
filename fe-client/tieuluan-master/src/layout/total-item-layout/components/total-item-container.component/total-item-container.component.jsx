import React, { useState } from "react";
import './total-item-container.styles.css';
import Item from '../../../../components/item.component/item.component'
import Pagination from "react-js-pagination";
import { useDispatch } from "react-redux";
import { fetchFilterPosts } from "../../../../redux/product/productSlice";
const TotalItemContainer = ({ lstPostSearch, Search }) => {
    const dispatch = useDispatch();
    const [currentPage, setCurrentPage] = useState(1);
    const onChangePage = (pageNumber) => {
        setCurrentPage(pageNumber)
        dispatch(fetchFilterPosts({
            Search,
            Page: pageNumber,
            Size: 12
        }));
    }
    return (
        <div>
            <main>
                <div className="no-padding" style={{ width: "100%" }}>
                    <div className="list-view">
                        {
                            lstPostSearch.posts && lstPostSearch.posts.length > 0 ? (
                                <div>
                                    <div style={{ display: "flex", flexFlow: "wrap" }}>
                                        {
                                            lstPostSearch.posts.map((item) => (
                                                <Item key={item.id} item={item} />
                                            ))
                                        }
                                    </div>
                                    <Pagination
                                        activePage={currentPage}
                                        itemsCountPerPage={24}
                                        totalItemsCount={lstPostSearch.totalSize}
                                        pageRangeDisplayed={5}
                                        onChange={(pageNumber) => onChangePage(pageNumber)}
                                    />
                                </div>)
                                : (
                                    <div>
                                        <div className="notfound">
                                            <div className="alert alert-warning">
                                            <img src="https://static.chotot.com/storage/empty_state/desktop/search_no_found_keyword.png" alt="PageNotFound" loading="lazy" height="200px" width="400px"/><br/>
                                                <b>Kh??ng t??m th???y k???t qu??? t??? kh??a ???? nh???p</b><br/>
                                                H??y ch???c ch???n r???ng t???t c??? c??c t??? ?????u ????ng ch??nh t???. H??y th??? nh???ng t??? kh??a kh??c ho???c nh???ng t??? kh??a chung h??n.</div>
                                        </div>
                                    </div>
                                )
                        }
                    </div>
                </div>
            </main>
        </div>
    )
}
export default TotalItemContainer;