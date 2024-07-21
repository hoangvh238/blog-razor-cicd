;(function($){
/**
 * jqGrid Phiên dịch tiếng Việt
 * Người dịch: [Tên của bạn]
 * Email: [Email của bạn]
 * http://blog.zakkum.com
 * Giấy phép: MIT và GPL
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
**/
$.jgrid = $.jgrid || {};
$.extend($.jgrid,{
	defaults : {
		recordtext: "{0}-{1} đang được liệt kê. Tổng cộng: {2}",
	    emptyrecords: "Không tìm thấy bản ghi",
		loadtext: "Đang tải...",
		pgtext : "Trang {0} / {1}"
	},
	search : {
	    caption: "Tìm kiếm...",
	    Find: "Tìm",
	    Reset: "Đặt lại",	    
	    odata: [{ oper:'eq', text:"bằng"},{ oper:'ne', text:"không bằng"},{ oper:'lt', text:"nhỏ hơn"},{ oper:'le', text:"nhỏ hơn hoặc bằng"},{ oper:'gt', text:"lớn hơn"},{ oper:'ge', text:"lớn hơn hoặc bằng"},{ oper:'bw', text:"bắt đầu bằng"},{ oper:'bn', text:"không bắt đầu bằng"},{ oper:'in', text:"trong"},{ oper:'ni', text:"không trong"},{ oper:'ew', text:"kết thúc bằng"},{ oper:'en', text:"không kết thúc bằng"},{ oper:'cn', text:"chứa"},{ oper:'nc', text:"không chứa"},{ oper:'nu', text:'là null'},{ oper:'nn', text:'không phải null'}],
	    groupOps: [	{ op: "VÀ", text: "tất cả" },	{ op: "HOẶC",  text: "bất kỳ" }],
		operandTitle : "Nhấp để chọn thao tác tìm kiếm.",
		resetTitle : "Đặt lại giá trị tìm kiếm"
	},
	edit : {
	    addCaption: "Thêm bản ghi",
	    editCaption: "Chỉnh sửa bản ghi",
	    bSubmit: "Gửi",
	    bCancel: "Hủy",
		bClose: "Đóng",
		saveData: "Dữ liệu đã thay đổi! Có muốn lưu không?",
		bYes : "Có",
		bNo : "Không",
		bExit : "Hủy",
	    msg: {
	        required:"Trường này là bắt buộc",
	        number:"Vui lòng nhập một số",
	        minValue:"Giá trị nhập vào phải lớn hơn hoặc bằng",
	        maxValue:"Giá trị nhập vào phải nhỏ hơn hoặc bằng",
	        email: "Đây không phải là một địa chỉ email hợp lệ",
	        integer: "Vui lòng nhập một số nguyên",
			url: "Đây không phải là một URL hợp lệ. Cần có tiền tố ('http://' hoặc 'https://')",
			nodefined : " không được định nghĩa!",
			novalue : " giá trị trả về là bắt buộc!",
			customarray : "Hàm tùy chỉnh phải trả về một mảng!",
			customfcheck : "Hàm tùy chỉnh phải có trong trường hợp kiểm tra tùy chỉnh!"
		}
	},
	view : {
	    caption: "Xem bản ghi",
	    bClose: "Đóng"
	},
	del : {
	    caption: "Xóa",
	    msg: "Có muốn xóa các bản ghi đã chọn không?",
	    bSubmit: "Xóa",
	    bCancel: "Hủy"
	},
	nav : {
		edittext: " ",
	    edittitle: "Chỉnh sửa hàng đã chọn",
		addtext:" ",
	    addtitle: "Thêm hàng mới",
	    deltext: " ",
	    deltitle: "Xóa hàng đã chọn",
	    searchtext: " ",
	    searchtitle: "Tìm bản ghi",
	    refreshtext: "",
	    refreshtitle: "Làm mới bảng",
	    alertcap: "Cảnh báo",
	    alerttext: "Vui lòng chọn một hàng",
		viewtext: "",
		viewtitle: "Xem hàng đã chọn"
	},
	col : {
	    caption: "Hiển thị/Ẩn cột",
	    bSubmit: "Gửi",
	    bCancel: "Hủy"	
	},
	errors : {
		errcap : "Lỗi",
		nourl : "URL chưa được cấu hình",
		norecords: "Không có bản ghi nào để thao tác",
	    model : "colNames độ dài <> colModel!"
	},
	formatter : {
		integer : {thousandsSeparator: " ", defaultValue: '0'},
		number : {decimalSeparator:".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00'},
		currency : {decimalSeparator:".", thousandsSeparator: " ", decimalPlaces: 2, prefix: "", suffix:"", defaultValue: '0.00'},
		date : {
			dayNames:   [
				"Cn", "T2", "T3", "T4", "T5", "T6", "T7",
				"Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy"
			],
			monthNames: [
				"Th1", "Th2", "Th3", "Th4", "Th5", "Th6", "Th7", "Th8", "Th9", "Th10", "Th11", "Th12",
				"Tháng Một", "Tháng Hai", "Tháng Ba", "Tháng Tư", "Tháng Năm", "Tháng Sáu", "Tháng Bảy", "Tháng Tám", "Tháng Chín", "Tháng Mười", "Tháng Mười Một", "Tháng Mười Hai"
			],
			AmPm : ["SA","CH","SA","CH"],
			S: function (j) {return j < 11 || j > 13 ? ['st', 'nd', 'rd', 'th'][Math.min((j - 1) % 10, 3)] : 'th'},
			srcformat: 'Y-m-d',
			newformat: 'd/m/Y',
			parseRe : /[#%\\\/:_;.,\t\s-]/,
			masks : {
	            ISO8601Long:"Y-m-d H:i:s",
	            ISO8601Short:"Y-m-d",
	            ShortDate: "n/j/Y",
	            LongDate: "l, F d, Y",
	            FullDateTime: "l, F d, Y g:i:s A",
	            MonthDay: "F d",
	            ShortTime: "g:i A",
	            LongTime: "g:i:s A",
	            SortableDateTime: "Y-m-d\\TH:i:s",
	            UniversalSortableDateTime: "Y-m-d H:i:sO",
	            YearMonth: "F, Y"
	        },
	        reformatAfterEdit : false
		},
		baseLinkUrl: '',
		showAction: '',
	    target: '',
	    checkbox : {disabled:true},
		idName : 'id'
	}
});
})(jQuery);
