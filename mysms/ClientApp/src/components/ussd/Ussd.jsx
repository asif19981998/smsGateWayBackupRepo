import React, { Component, useEffect, useState } from 'react';
import { Form } from 'reactstrap';
import axios from "axios";
import Select from "react-select";
import CreatableSelect from 'react-select/creatable';
import * as ReactBootStrap from 'react-bootstrap';
import { useToasts } from "react-toast-notifications";
import { blue } from '@mui/material/colors';

const UssdAdd = () => {
    const [sendSms, setSendSms] = useState({ id: 0, port: "", content: "" });
    const [clientName, setClientName] = useState("");
    const [contactList, setCotactList] = useState([]);
    const [selectedOption, setSelectedOption] = useState([]);
    const [loading, setLoading] = useState(false);
    const { addToast } = useToasts();
    const [balance, setBalance] = useState("");
    const [ports, setPorts] = useState([]);
    
   
    var incrementBar = 0;
    /*const [selectedOptions, setSelectedOptions] = useState([]);*/
  
    

    const url = "https://localhost:44331/api/contact/GetContactForDD";
    /*const sendUrl = "https://localhost:44331/api/sms";*/
    useEffect(() => {
        axios.get(url).then(result => {
            setCotactList(result.data)
            console.log(contactList)
        })

        axios.get("api/ussd/getAvailablePort").then(res => { setPorts(res.data) })



    },[])

    const handleInputChange = (e) => {

        const { name, value } = e.target;

        const fieldsValue = { [name]: value };
        setSendSms({ ...sendSms, ...fieldsValue });
    }
    const handleItemAdded = (phoneNo, allItems) => {

        console.log(typeof phoneNo);
        axios.post("api/contact/searchByPhoneNo", { phoneNo: phoneNo }).then(res => setClientName(res.data))
        console.log(phoneNo);
    }
   
    const handleSendSms = (e) => {
        e.preventDefault();
        console.log(selectedOption);
        sendSms.phoneNo = selectedOption;
        incrementBar = 100 / selectedOption.length;
       
        axios.post("api/Ussd", sendSms).then(result => {
            setBalance(result.data);
            setLoading(false);
            addToast(result.data, {
                appearance: "success"
            });
        })
        setLoading(true);
        
      
       
     }

    /*const handleProgressBar = */
    const handlePhoneNoChange = selectedOption => {
        console.log(selectedOption)
        
        var optionData = [];
        selectedOption.forEach(option => {
            if (option.phoneNo) optionData.push(option.phoneNo)
            else optionData.push(option.value)
           
        })
        setSelectedOption(optionData)
      
    };

    const handleSearchAbleChange = (e) => {
        console.log(e.value);
    }
   
    return (
        <>
        
           
          
            {loading ?
                <div style={{ position: "fixed", top: "50%", left: "50%", zIndex: 9999}}>
                    <ReactBootStrap.Spinner animation="border" />
                    </div>
                
                : ""}
            <div className={`${loading ? "sendsms-form":""}`}>
                <h1>USSD Send</h1>
                <hr />



                <div className="row">
                    <div className="col-md-6">

                        <form className>
                            <div className="form-group">
                                <label htmlFor="port">Port</label>

                                <select name="port" id="port" className="form-control" onChange={handleInputChange}>
                                    <option >---select a port---</option>
                                    {ports.map((port) => (
                                        <option value={port.number} >{port.label}</option>
                                    ))}

                                </select>
                            </div>


                            <div className="form-group">
                                <label >Message:</label>
                                <textarea className="form-control" name="content" rows="8" cols="8" onChange={handleInputChange} />
                            </div>

                            <button className="btn btn-danger save-btn" onClick={handleSendSms}>Save</button>
                        </form>

                    </div>
                    <div className="col-md-6">
                        <div className="card" >
                            <h2 style={{ color:"blue" }} >{balance}</h2>
                        </div>

                    </div>
                </div>
                </div>
          

       </>
    )
}

export default UssdAdd;