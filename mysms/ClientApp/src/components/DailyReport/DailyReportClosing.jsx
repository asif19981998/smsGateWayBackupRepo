import React, { useState, useEffect, useRef } from "react";
import MaterialTable from "material-table";
import AddBox from '@material-ui/icons/AddBox';
import ArrowDownward from '@material-ui/icons/ArrowDownward';
import Check from '@material-ui/icons/Check';
import ChevronLeft from '@material-ui/icons/ChevronLeft';
import ChevronRight from '@material-ui/icons/ChevronRight';
import Clear from '@material-ui/icons/Clear';
import DeleteOutline from '@material-ui/icons/DeleteOutline';
import Edit from '@material-ui/icons/Edit';
import FilterList from '@material-ui/icons/FilterList';
import FirstPage from '@material-ui/icons/FirstPage';
import LastPage from '@material-ui/icons/LastPage';
import Remove from '@material-ui/icons/Remove';
import SaveAlt from '@material-ui/icons/SaveAlt';
import Search from '@material-ui/icons/Search';
import ViewColumn from '@material-ui/icons/ViewColumn';
import { forwardRef } from 'react';
import * as ReactBootStrap from 'react-bootstrap';
import axios from 'axios'
import XLSX from 'xlsx';
import { error } from "jquery";
import { useToasts } from "react-toast-notifications";
/*import "./Reporting.css";*/
export default function DailyReportClosing() {

    const [searchModel, setSearchModel] = useState({ reportDate: new Date(), type: "" })
    var sendSms = { id: 0, systemId: "", smsType: "" };
    const checkBoxRef = useRef(null);
    const [errors, setErrors] = useState({})
    const [reportDate, setReportDate] = useState("");
    const [updateDate, setUpdateDate] = useState("");
    var report_date_and_title = `Daily Closing Report(Stop Watching)_${reportDate}`
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState([{}]);
    const url = "api/DailyReport/getClosingReport";
    const { addToast } = useToasts();
    var reportModel = { id: 0, systemId: "", objectName: "", arc: "", sms: "", remarks: "", lastUpdateTime: new Date() }

    var searchData = { SearchValue: "" };
    var seletedRows = [];

    const tableIcons = {
        Add: forwardRef((props, ref) => <AddBox {...props} ref={ref} />),
        Check: forwardRef((props, ref) => <Check {...props} ref={ref} />),
        Clear: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
        Delete: forwardRef((props, ref) => <DeleteOutline {...props} ref={ref} />),
        DetailPanel: forwardRef((props, ref) => <ChevronRight {...props} ref={ref} />),
        Edit: forwardRef((props, ref) => <Edit {...props} ref={ref} />),
        Export: forwardRef((props, ref) => <SaveAlt {...props} ref={ref} />),
        Filter: forwardRef((props, ref) => <FilterList {...props} ref={ref} />),
        FirstPage: forwardRef((props, ref) => <FirstPage {...props} ref={ref} />),
        LastPage: forwardRef((props, ref) => <LastPage {...props} ref={ref} />),
        NextPage: forwardRef((props, ref) => <ChevronRight {...props} ref={ref} />),
        PreviousPage: forwardRef((props, ref) => <ChevronLeft {...props} ref={ref} />),
        ResetSearch: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
        Search: forwardRef((props, ref) => <Search {...props} ref={ref} />),
        SortArrow: forwardRef((props, ref) => <ArrowDownward {...props} ref={ref} />),
        ThirdStateCheck: forwardRef((props, ref) => <Remove {...props} ref={ref} />),
        ViewColumn: forwardRef((props, ref) => <ViewColumn {...props} ref={ref} />)
    };


    const validate = (fieldValues = searchModel) => {
        let temp = { ...errors }
        if ('type' in fieldValues) {
            temp.type = "";
            temp.type += fieldValues.type ? "" : "This field is Required"
        }

        setErrors({
            ...temp
        })

        if (fieldValues == searchModel)
            return Object.values(temp).every(x => x == "")

    }

    const handleRowChange = (rows) => {
        seletedRows = rows;

    }

    const handleSeletedRowsSms = (smsType) => {
        console.log(seletedRows)
        seletedRows.forEach(row => {


            console.log(loading)
            let selectedSearchData = { SearchValue: "" };
            selectedSearchData.SearchValue = row.systemId;
            let portList = [];
            var sendSms = { id: 0, port: 0, phoneNo: [], content: "" };




            axios.post("api/contact/searchBySynonym", selectedSearchData).then(result => {

                var contact = result.data.result;
                if (result.data.isSuccess) {

                    axios.get("api/portSetting").then(result => {

                        portList = result.data[0];
                        if (portList.port_1 == contact.sender) sendSms.port = "1";
                        if (portList.port_2 == contact.sender) sendSms.port = "2";
                        if (portList.port_3 == contact.sender) sendSms.port = "3";
                        if (portList.port_4 == contact.sender) sendSms.port = "4";
                        if (sendSms.port == 0) {
                            addToast("Port is not Avialable", {
                                appearance: "error"
                            });


                        }

                        var temp = [];
                        temp.push(contact.phoneNo);
                        sendSms.phoneNo = temp;


                        if (smsType == "w") {
                            sendSms.content = contact.watch;
                        }
                        if (smsType == "c") {
                            sendSms.content = contact.close;
                        }
                        if (smsType == "s") {
                            sendSms.content = contact.status;
                        }

                        axios.post("api/sms", sendSms).then(result => {

                            if (result.data == "Successfully Send") {
                                removeCheckBoxActiveClass()
                                setLoading(false)
                                addToast(result.data, {
                                    appearance: "success"
                                });
                            }

                            else {
                                addToast(result.data, {
                                    appearance: "error"
                                });
                            }

                        })


                    })

                }

                else {
                    addToast("Unable To Send", {
                        appearance: "error"
                    });
                }


            })

        })
        setLoading(true)

    }

    const handleSendSms = (row, smsType) => {
        searchData.SearchValue = row.systemId;
        let portList = [];
        var sendSms = { id: 0, port: 0, phoneNo: [], content: "" };
        axios.post("api/contact/searchBySynonym", searchData).then(result => {
            var contact = result.data.result;
            if (result.data.isSuccess) {

                axios.get("api/portSetting").then(result => {

                    portList = result.data[0];
                    if (portList.port_1 == contact.sender) sendSms.port = "1";
                    if (portList.port_2 == contact.sender) sendSms.port = "2";
                    if (portList.port_3 == contact.sender) sendSms.port = "3";
                    if (portList.port_4 == contact.sender) sendSms.port = "4";
                    if (sendSms.port == 0) {
                        addToast("Port is not Avialable", {
                            appearance: "error"
                        });
                        setLoading(false)
                        return;
                    }

                    var temp = [];
                    temp.push(contact.phoneNo);
                    sendSms.phoneNo = temp;


                    if (smsType == "w") {
                        sendSms.content = contact.watch;
                    }
                    if (smsType == "c") {
                        sendSms.content = contact.close;
                    }
                    if (smsType == "s") {
                        sendSms.content = contact.status;
                    }

                    axios.post("api/sms", sendSms).then(result => {
                        setLoading(false)
                        if (result.data == "Successfully Send") {

                            addToast(result.data, {
                                appearance: "success"
                            });
                        }

                        else {
                            addToast(result.data, {
                                appearance: "error"
                            });
                        }

                    })

                    setLoading(true)
                })

            }

            else {
                addToast("Unable To Send", {
                    appearance: "error"
                });
            }


        })



    }



    const [columns, setColumns] = useState([
        { title: 'SystemId', field: 'systemId', editable: 'never' },
        { title: 'Object Name', field: 'objectName', editable: 'never' },
        { title: "Phone No", field: 'phoneNo', editable: 'never' },
        { title: 'ARC', field: 'arc', editable: 'never' },
        { title: 'Arc Closing Time', field: 'arcStopTime', editable: 'never' },
        { title: 'SMS', field: 'sms', editable: 'never', editable: 'onUpdate' },
        { title: 'Sms Closing Time', field: 'smsStopTime', editable: 'never' },
        { title: 'Remarks', field: 'remarks', editable: 'onUpdate' },
        {
            title: 'Send SMS',
            field: 'remarks',
            filtering: false,
            editable: 'never',
            render: row => (
                <>
                    <button className="btn btn-success" onClick={() => handleSendSms(row, "w")} style={{ marginRight: "1px", marginBottom: "2px" }}>Watch</button>
                    <button className="btn btn-info" onClick={() => handleSendSms(row, "c")} style={{ marginRight: "1px", marginBottom: "2px" }}>Close</button>
                    <button className="btn btn-danger" onClick={() => handleSendSms(row, "s")} style={{ marginRight: "1px", marginBottom: "2px" }}>Status</button>
                </>
            )
        }

    ]);

    const handleUpdateRow = (updateRemoteModel) => {
        reportModel.id = updateRemoteModel.id;
        reportModel.systemId = updateRemoteModel.systemId;
        reportModel.objectName = updateRemoteModel.objectName;
        reportModel.arc = updateRemoteModel.arc;
        reportModel.sms = updateRemoteModel.sms;
        reportModel.remarks = updateRemoteModel.remarks;
        reportModel.lastUpdateTime = updateRemoteModel.lastUdpateTime;


        axios.put("api/DailyReport", reportModel).then(res => console.log(res.data))

    }

    const handleReportSearchChange = (e) => {
        const { name, value } = e.target;
        const fieldValue = { [name]: value };
        setSearchModel({ ...searchModel, ...fieldValue })
        validate(fieldValue)
        if (name == "reportDate") {
            setUpdateDate(value);
        }
    }

    const handledownloadExcel = () => {
        console.log("called")
        const newData = data.map(row => {
            delete row.tableData
            return row
        })
        const workSheet = XLSX.utils.json_to_sheet(newData)
        const workBook = XLSX.utils.book_new()
        XLSX.utils.book_append_sheet(workBook, workSheet, "closingreport")

        let buf = XLSX.write(workBook, { bookType: "xlsx", type: "buffer" })

        XLSX.write(workBook, { bookType: "xlsx", type: "binary" })

        XLSX.writeFile(workBook, "closingreport.xlsx")


    }

    const handleUpdateReport = (e) => {

        e.preventDefault();
        if (validate()) {
            axios.post("api/DailyReport/getUpdatedClosingReport", searchModel).then(res => {

                setLoading(false)
                setData(res.data)
                setReportDate(updateDate);
            })
            setLoading(true)
        }

    }

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = yyyy + '-' + mm + '-' + dd;

    const removeCheckBoxActiveClass = () => {
        data.forEach(d => { if (d.tableData) d.tableData.checked = false });

    }


    useEffect(() => {
        
        axios.get("api/DailyReport/setDailyReportClosingData");

        axios.get(url).then(res => {
            setLoading(false)
            setData(res.data)
            console.log(res.data)

        })
        setLoading(true)

    }, [])

    const handlePageChange = (page, pageSize) => {
        console.log(page)
        console.log(pageSize)
    }

    return (

        <>
            {loading ?
                <div style={{ position: "fixed", top: "50%", left: "50%", zIndex: 9999 }}>
                    <ReactBootStrap.Spinner animation="border" /><span style={{ color: "red", fontSize: "55px", fontWeight: "bold" }}>Loading...</span><span style={{ color: "red", fontWeight: "bold" }}>Take max 30 Sec</span>
                </div>

                : ""}

            {data.length <= 1 ? <div>No Data Found</div> :
                <>
                    <div>
                        <div className="row">
                            <div className="col-md-3">
                                <div className="form-group">
                                    <label>Date: </label>
                                    <input type="datetime-local" name="reportDate" max={today} id="dateInput" className="form-control" onChange={handleReportSearchChange} />
                                </div>

                            </div>
                            <div className="col-md-3">
                                <div className="row">
                                    <div>
                                        <div className="col-md-6">
                                            <div className="form-group">
                                                <label htmlFor="type">Type</label>

                                                <select name="type" id="type" className="form-control" style={{ border: errors.type ? "1px solid red" : "1px solid #ced4da" }} onChange={handleReportSearchChange} required>
                                                    <option value="">---select a Type---</option>
                                                    <option value="arc">ARC</option>
                                                    <option value="sms">SMS</option>
                                                </select>
                                                {errors.type && <div style={{ color: "red" }}>{errors.type}</div>}


                                            </div>
                                        </div>
                                        {/*<div className="col-md-6">*/}
                                        {/*    <div className="form-group">*/}
                                        {/*        <label htmlFor="time">Time</label>*/}
                                        {/*        <input type="time" id="time" name="time"/>*/}
                                        {/*    </div>*/}
                                        {/*</div>*/}
                                    </div>

                                </div>
                            </div>
                            <div className="col-md-3">

                                <button className="btn btn-success" style={{ marginTop: "32px", marginRight: "1px", marginBottom: "2px" }} onClick={handleUpdateReport}>Update</button>
                                <button className="btn btn-danger" style={{ marginTop: "32px", marginRight: "1px", marginBottom: "2px" }} onClick={handledownloadExcel}>Export</button>
                            </div>
                            <div className="col-md-3" style={{ marginTop: "32px" }}>
                                <button className="btn btn-success" onClick={() => handleSeletedRowsSms("w")} style={{ marginRight: "1px", marginBottom: "2px" }}>Watch</button>
                                <button className="btn btn-info" onClick={() => handleSeletedRowsSms("c")} style={{ marginRight: "1px", marginBottom: "2px" }}>Close</button>
                                <button className="btn btn-danger" onClick={() => handleSeletedRowsSms("s")} style={{ marginRight: "1px", marginBottom: "2px" }}>Status</button>

                            </div>

                        </div>

                    </div>
                    <MaterialTable
                        ref={checkBoxRef}
                        icons={tableIcons}
                        title={report_date_and_title}
                        columns={columns}
                        data={data.map((data) => data)}
                        options={{
                            pageSize: 500,
                            pageSizeOptions: [500, 1000, 3000],
                            filtering: true,
                            selection: true,
                            paginationPosition:"top"
                        }}

                        editable={{

                            onRowUpdate: (newData, oldData) =>
                                new Promise((resolve, reject) => {
                                    {

                                        const dataUpdate = [...data];
                                        const index = oldData.tableData.id;
                                        dataUpdate[index] = newData;
                                        setData([...dataUpdate]);
                                        handleUpdateRow(newData)
                                        resolve();
                                    }
                                }),

                        }}
                        onChangePage={(page, pageSize) => handlePageChange(page, pageSize)}
                        onSelectionChange={(rows) => handleRowChange(rows)}
                        

                    />
                </>
            }


        </>
    )
}