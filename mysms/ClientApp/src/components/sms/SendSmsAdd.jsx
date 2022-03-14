import React, {  useEffect, useState,useRef } from 'react';
import { Form } from 'reactstrap';
import axios from "axios";
import Select from "react-select";
import CreatableSelect from 'react-select/creatable';
import * as ReactBootStrap from 'react-bootstrap';
import { useToasts } from "react-toast-notifications";
import "./SendSms.css"
const SendSmsAdd = () => {
    const [sendSms, setSendSms] = useState({ id: 0, port: 0, phoneNo: [], content: "" });
    const [initialValues, setInitialValues] = useState({ id: 0, port: 0, phoneNo: [], content: "" })
    const selectFieldRef = useRef();
    const [clientName, setClientName] = useState("");
    const [contactList, setCotactList] = useState([]);
    const [selectedOption, setSelectedOption] = useState([]);
    const [loading, setLoading] = useState(false);
    const { addToast } = useToasts();
    const [ports, setPorts] = useState([]);
  
   

    

    const url = "api/contact/GetContactForDD";
    const sendUrl = "api/sms";
    useEffect(() => {
        axios.get(url).then(result => {
            setCotactList(result.data)
           
        })

        axios.get("api/sms/getAvailablePort").then(res => { setPorts(res.data)})

    },[])

    const handleInputChange = (e) => {

        const { name, value } = e.target;

        const fieldsValue = { [name]: value };
        setSendSms({ ...sendSms, ...fieldsValue });
    }

    const resetForm = () => {

        setSendSms({ ...initialValues })
        selectFieldRef.current.clearValue();
       
    }
   
   
    const handleSendSms = (e) => {
        e.preventDefault();
        
        sendSms.phoneNo = selectedOption;
        resetForm();
       
        axios.post(sendUrl, sendSms).then(result => {
           
            setLoading(false);
            resetForm();
            addToast(result.data, {
                appearance: "success"
            });
        })
        setLoading(true);
        
      
       
     }

    
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
                <h1>Sms Send</h1>
                <form className>
                    <div className="form-group">
                        <label htmlFor="port">Port</label>

                        <select name="port" id="port" className="form-control" onChange={handleInputChange} value={sendSms.port} >
                            <option value="0">---select a port---</option>
                            {ports.map((port) => (
                                <option value={port.number} >{port.label}</option>
                               ))}
                          
                         </select>
                    </div>

                    <div className="form-group">
                        <label htmlFor="phoneNo">Phone Number</label>
                        <CreatableSelect
                            isMulti
                            ref={selectFieldRef}
                            name="phoneNo"
                            onChange={handlePhoneNoChange}
                            options={contactList}
                            
                            
                        />
                        {/* options={contactList.map((data) => data)}*/}
                    </div>
                    <div className="form-group">
                        <label >Message:</label>
                        <textarea className="form-control" name="content" rows="8" cols="8" onChange={handleInputChange} value={sendSms.content} />
                    </div>

                    <button className="btn btn-danger save-btn" onClick={handleSendSms}>Send</button>
                </form>
                </div>
          

           

        </>
    )
}

export default SendSmsAdd;