export function updateStatus(status) {
    if (status === 1)
        return {
            className: 'text-warning',
            text: 'Chờ xử lý'
        }
    else if (status === 2)
        return {
            className: 'text-success',
            text: 'Đang hiển thị'
        }
    else if (status === 3)
        return {
            className: 'text-primary',
            text: 'Đã từ chối'
        }
    else if (status === 4)
        return {
            className: 'text-danger',
            text: 'Đã khoá'
        }
    else
        return {}
}

export function updatePostType(postType) {
    let result = ""
    if (postType === 1)
        result = 'Cần bán'
    else if (postType === 2)
        result = 'Cần mua'
    else if (postType === 3)
        result = 'Cho thuê'
    else if (postType === 4)
        result = 'Cần thuê'

    return result;
}

export function updateLocation(provinces, address, provinceID, districtID, wardID) {
    let data = []
    if (!Array.isArray(provinces)) return data;

    let province = provinces.find(x => x.code === provinceID);
    if (address)
        data.push(address);
    
    if (province) {
        let district = province.districts.find(x => x.code === districtID);
        if (district) {
            let ward = district.wards.find(x => x.code === wardID);
            if (ward) {
                data.push(ward.name);
            }
            data.push(district.name);
        }
        data.push(province.name);
    }
    return data
}

export function addLeadingZeros(num, totalLength = 7) {
    return `#${String(num).padStart(totalLength, '0')}`;
}

export function formatPrice(price) {
    let result = ""
    if (!isNaN(price)) {
        if (price / 1000000000 > 1)
            result = (price / 1000000000).toFixed(1) + " tỷ";
        else if (price / 1000000 > 1)
            result = (price / 1000000).toFixed(1) + " triệu";
        else
            result = (price + " đồng");
    }
    return result
}

export function updatePaper(paperID) {
    let result = ""
    switch (paperID) {
        case 1:
            result = "Sổ đỏ";
            break;
        case 2:
            result = "Sổ hồng";
            break;
        case 3:
            result = "Sổ trắng";
            break;
        case 4:
            result = "Giấy chứng nhận quyền sở hữu";
            break;
        case 5:
            result = "Giấy tờ hợp lệ";
            break;
        case 6:
            result = "Chưa xác định";
            break;
        default:
    }

    return result;
}

export function updateDirection(directionID) {
    let result = ""
    switch (directionID) {
        case 1:
            result = "Đông";
            break;
        case 2:
            result = "Tây";
            break;
        case 3:
            result = "Nam";
            break;
        case 4:
            result = "Bắc";
            break;
        case 5:
            result = "Đông - Bắc";
            break;
        case 6:
            result = "Tây - Bắc";
            break;
        case 7:
            result = "Đông - Nam";
            break;
        case 8:
            result = "Tây - Nam";
            break;
        default:
    }

    return result;
}

export function updateDateTime(date) {
    try {
        const dateTime = new Date(date);

        return dateTime;
    }
    catch {
        return new Date();
    }
}

export function formatDateTimeDDmmYYYY(date) {
    const dateTime = updateDateTime(date);

    let dateString = `${dateTime.getDate()}/${dateTime.getMonth() + 1}/${dateTime.getFullYear()}`;
    return dateString
}

export function formatDateTimeDDmmYYYYHHMM(date) {
    const dateTime = updateDateTime(date);

    let dateString = `${dateTime.getDate()}/${dateTime.getMonth() + 1}/${dateTime.getFullYear()}, ${dateTime.getHours()}:${dateTime.getMinutes()}`;
    return dateString
}

export function formatDateTimeYYYYmmdd(date) {
    const dateTime = updateDateTime(date);

    let dateString=""
    if (dateTime.getDate() < 10)
        dateString = `${dateTime.getFullYear()}-${dateTime.getMonth() + 1}-0${dateTime.getDate()}`;
    else
        dateString = `${dateTime.getFullYear()}-${dateTime.getMonth() + 1}-${dateTime.getDate()}`;
    return dateString
}

export function convertGenderToString(gender) {
    if (gender)
        return 'female'
    else
        return 'male'
}

export function convertGenderToBoolean(gender) {
    if (gender === 'female')
        return true
    else
        return false
}