import React, { useState, useEffect } from "react";
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
import "./Contact.css"
export default function ContactList() {
    const [contactModel, setContactModel] = useState({ id: 0, name: "", email: "", phoneNo: "", watch: "", close: "", status: "", sender:"" });
    const [data, setData] = useState([{}]);
    var sendSms = { id: 0, port: 0, phoneNo: [], content: "" };
    const [loading, setLoading] = useState(false);
    const { addToast } = useToasts();
    const url = "api/contact";


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

    const handleSendSms = (row, smsType) => {
        let portList = [];
        axios.get("api/portSetting").then(result => {

            portList = result.data[0];
            if (portList.port_1 == row.sender) sendSms.port = "1";
            if (portList.port_2 == row.sender) sendSms.port = "2";
            if (portList.port_3 == row.sender) sendSms.port = "3";
            if (portList.port_4 == row.sender) sendSms.port = "4";
            if (sendSms.port == 0) {
                addToast("Port is not Avialable", {
                    appearance: "error"
                });
                setLoading(false)
                return;
            }
            var temp = [];
            temp.push(row.phoneNo);
            sendSms.phoneNo = temp;
           
            if (smsType == "w") {
                sendSms.content = row.watch;
            }
            if (smsType == "c") {
                sendSms.content = row.close;
            }
            if (smsType == "s") {
                sendSms.content = row.status;
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
         
        })
       
        setLoading(true)
        


    }
    


    const [columns, setColumns] = useState([
        { title: 'Name', field: 'name' },
        { title: 'Email', field: 'email'},
        { title: 'Device Sim Number', field: 'phoneNo' },
        { title: 'Watch', field: 'watch' },
        { title: 'Close', field: 'close' },
        { title: 'Status', field: 'status' },
        { title: 'CMC Sim No', field: 'sender' },

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
        },


    ]);

  

   

    const handledownloadExcel = () => {
        
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


    const handleUpdateRow = (updateRemoteModel) => {
        var sendModel = { ...contactModel, ...updateRemoteModel }
        axios.put("api/contact", sendModel).then(res => {
            if (res.data == "Success") {
                
                addToast("Successfully Updated", {
                    appearance: "success"
                });
            }

            else {
                
                addToast(res.data, {
                    appearance: "error"
                });
            }

        })
    }
    




    useEffect(() => {
        const abortController = new AbortController();
        axios.get(url, {
            signal: abortController.signal
        }).then(result => {
            setLoading(false)
            setData(result.data);
            
        })
        setLoading(true)
        return () => {
            abortController.abort();
        };
    }, [])
    
    //useEffect(() => {
    //   var tableData = (query) => (
    //        new Promise((resolve, reject) => {
    //            let url = "api/contact";
    //            //url += "per_page=" + query.pageSize;
    //            //url += "&page=" + (query.page + 1);
    //            fetch(url)
    //                .then((response) => response.json())
    //                .then((result) => {
    //                    resolve({
    //                        data: result.data,
    //                        page: result.page - 1,
    //                        totalCount: result.total,
    //                    });
    //                });
    //        }));

    //    setData(tableData)

    //})


    return (

        <>
           
            {loading ?
                <div style={{ position: "fixed", top: "50%", left: "50%", zIndex: 9999 }}>
                    <ReactBootStrap.Spinner animation="border" /><span style={{ color: "red", fontSize: "55px", fontWeight: "bold" }}>Loading...</span><span style={{ color: "red", fontWeight: "bold" }}>Take max 30 Sec</span>
                </div>

                : ""}
            <MaterialTable
                icons={tableIcons}
                title="Contact List"
                columns={columns}
                data={data}
                options={{
                    pageSize: 500,
                    pageSizeOptions: [500,1000,3000],
                    filtering: true,
                    
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
            />

        </>
    )
}
