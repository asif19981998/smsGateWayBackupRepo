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


export default function GroupWiseSms() {
    var groupWiseSms = { id: "", port: "", phoneNo: "", content: "" };
    const [data, setData] = useState([]);
    var searchData = { SearchValue: "" };
    const [loading, setLoading] = useState(false);
    const { addToast } = useToasts();
    const url = "api/group";


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


    const handleSendSms = (row) => {
        searchData.SearchValue = row.name;
        var contactList = [];
        var groupWiseSendSmsList = [];
        var portList = [];
        axios.post("api/contact/searchByGroupName", searchData).then(result => {
           
            contactList = result.data.result;
            console.log(contactList)
            if (result.data.isSuccess) {

                axios.get("api/portSetting").then(result => {

                    portList = result.data[0];
                    contactList.forEach(contact => {
                        groupWiseSms.id = contact.id;
                        groupWiseSms.phoneNo = contact.phoneNo;
                        groupWiseSms.content = contact.status;

                        if (portList.port_1 == contact.sender) groupWiseSms.port = "1";
                        if (portList.port_2 == contact.sender) groupWiseSms.port = "2";
                        if (portList.port_3 == contact.sender) groupWiseSms.port = "3";
                        if (portList.port_4 == contact.sender) groupWiseSms.port = "4";

                        if (Math.floor(groupWiseSms.port) > 0) {
                            groupWiseSendSmsList.push(groupWiseSms);
                        }


                        groupWiseSms = {};

                    })
                    
                    //var temp = [
                    //    { content: "Hi Rakib", id: 11776, phoneNo: "+8801706634346", port: "4" },
                    //    { content: "Hi Asif", id: 11775, phoneNo: "+8801844143531", port: "4" }
                    //]
                   
                   
                    axios.post("api/sms/GroupWiseSms", groupWiseSendSmsList).then(result => {
                        setLoading(false)
                        if (Math.floor(result.data)>0) {

                            addToast(`Send ${groupWiseSendSmsList.length} Sms , Successfully Send ${result.data}`, {
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
                    console.log(groupWiseSendSmsList);
                    })
                   
            }

            //else {
            //    addToast("Unable To Send", {
            //        appearance: "error"
            //    });
            //}


        })


    }


    const [columns, setColumns] = useState([
        { title: 'Name', field: 'name' },
        {
            title: 'Send SMS',
            field: 'remarks',
            filtering: false,
            editable: 'never',
            render: row => (
                <>
                   
                    <button className="btn btn-danger" onClick={() => handleSendSms(row)} style={{ marginRight: "1px", marginBottom: "2px" }}>Status</button>
                </>
            )
        }

    ]);




    



    useEffect(() => {
        axios.get(url).then(result => {
            setData(result.data.result)
        })

    }, [])


    return (

        <>

            {loading ?
                <div style={{ position: "fixed", top: "50%", left: "50%", zIndex: 9999 }}>
                    <ReactBootStrap.Spinner animation="border" /><span style={{ color: "red", fontSize: "55px", fontWeight: "bold" }}>Loading...</span><span style={{ color: "red", fontWeight: "bold" }}>Take max 30 Sec</span>
                </div>

                : ""}

            <div className="row">
               
                <div >
                    <MaterialTable
                        icons={tableIcons}
                        title="Group Wise Send List"
                        columns={columns}
                        data={data.map((data) => data)}
                        options={{
                            pageSize: 20,
                            pageSizeOptions: [10, 20, 50, 100],
                            filtering: true,

                        }}

                       
                    />
                </div>


            </div>


        </>
    )
}
