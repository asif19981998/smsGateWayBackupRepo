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
import GroupAdd from "./groupAdd";

export default function GroupList() {
    const [group, setGroup] = useState({ id: 0, name: "" });
    const [data, setData] = useState([]);
   
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





    const [columns, setColumns] = useState([
        { title: 'Name', field: 'name' },
       

    ]);


    const handledownloadExcel = () => {

        const newData = data.map(row => {
            delete row.tableData
            return row
        })
        const workSheet = XLSX.utils.json_to_sheet(newData)
        const workBook = XLSX.utils.book_new()
        XLSX.utils.book_append_sheet(workBook, workSheet, "groupList")

        let buf = XLSX.write(workBook, { bookType: "xlsx", type: "buffer" })

        XLSX.write(workBook, { bookType: "xlsx", type: "binary" })

        XLSX.writeFile(workBook, "groupList.xlsx")


    }


    const handleUpdateRow = (updateRemoteModel) => {
        var groupData = { ...group, ...updateRemoteModel }
        axios.put("api/group", groupData).then(res => {
            console.log(res.data.isSuccess)
            if (res.data.isSuccess) {
             
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

    const updateTableData = (tableData) => {
        setData(tableData);
    }



    useEffect(() => {
        axios.get(url).then(result => {
            updateTableData(result.data.result)
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
                <div className="col-md-6">
                    <GroupAdd updateTableData={updateTableData}/>
                </div>
                <div className="col-md-6">
                    <MaterialTable
                        icons={tableIcons}
                        title="Group List"
                        columns={columns}
                        data={data.map((data) => data)}
                        options={{
                            pageSize: 20,
                            pageSizeOptions: [10, 20, 50, 100],
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
                </div>
                
               
            </div>
            

        </>
    )
}
