import React, { useEffect, useState } from "react";
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
import axios from "axios";
import { useToasts } from "react-toast-notifications";

const PortSetting = () => {
    const [portSettings, setPortSetting] = useState([]);
    const [data, setData] = useState([]);
    const { addToast } = useToasts();
    const url = "api/PortSetting"

    const table_title = "Port Setting";
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
    var portModel = {id:0,port_1:"",port_2:"",port_3:"",port_4:""}

    const handleUpdateRow = (updateRemoteModel) => {
        console.log(updateRemoteModel);
        portModel.id = updateRemoteModel.id;
        portModel.port_1 = updateRemoteModel.port_1;
        portModel.port_2 = updateRemoteModel.port_2;
        portModel.port_3 = updateRemoteModel.port_3;
        portModel.port_4 = updateRemoteModel.port_4;
        axios.put(url, portModel).then(result => {
            if (result.data == null) {
                addToast("Updated Failed", {
                    appearance: "error"
                });
            }
            else {
                setData(result.data)                         
                addToast("Successfully Updated", {
                    appearance: "success"
                });
            }
        }

       );

    }

    const [columns, setColumns] = useState([
        { title: 'Port 1', field: 'port_1'},
        { title: 'Port 2', field: 'port_2'},
        { title: 'Port 3', field: 'port_3'},
        { title: 'Port 4', field: 'port_4'},
        


    ]);


    useEffect(() => {
        
        axios.get(url).then(result => {
            setData(result.data);
            
        })
    },[])

    return (
        <>
        <MaterialTable
            icons={tableIcons}
            title={table_title}
            columns={columns}
            data={data.map((data) => data)}
            //options={{
            //    pageSize: 20,
            //    pageSizeOptions: [10, 20, 50, 100]
            //}}
            editable={{

                onRowUpdate: (newData, oldData) =>
                    new Promise((resolve, reject) => {
                        {

                            const dataUpdate = [...data];
                            const index = oldData.tableData.id;
                            dataUpdate[index] = newData;
                            setPortSetting([...dataUpdate]);
                            handleUpdateRow(newData)
                            resolve();
                        }
                    }),

            }}
        />
        </>

    )


}


export default PortSetting;