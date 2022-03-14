import React, { Component, useEffect, useState } from 'react';
import { Form } from 'reactstrap';
import axios from "axios";
import { useToasts } from "react-toast-notifications";
const ContactAdd = () => {
    const { addToast } = useToasts();
    const [contactModel, setContactModel] = useState({ id: 0, name: "", email: "", phoneNo: "", watch: "", close: "", status: "", sender: "", Synonym:""});
    const [initialState, setInitialState] = useState({ id: 0, name: "", email: "", phoneNo: "", watch: "", close: "", status: "", sender: "", Synonym: "" });
    const [errors, setErrors] = useState({})
    const [portList, setPortList] = useState([]);

    const resetForm = () => {
        setContactModel(initialState);
    }


    const validate = (fieldValues = contactModel) => {
        let temp = { ...errors }
        if ('name' in fieldValues) {
            temp.name = "";
            temp.name += fieldValues.name ? "" : "This field is Required"
        }
        if ('synonym' in fieldValues) {
            temp.synonym = "";
            temp.synonym += fieldValues.synonym ? "" : "This field is Required"
        }
        if ('email' in fieldValues) {
            temp.email = "";
            temp.email += fieldValues.email ? "" : "This field is Required"
        }
        if ('phoneNo' in fieldValues) {
            temp.phoneNo = "";
            var pattern = new RegExp(/(^(\+88|0088)?(01){1}[3456789]{1}(\d){8})$/);
            temp.phoneNo += fieldValues.phoneNo ? "" : "This field is Required "
            temp.phoneNo += pattern.test(fieldValues.phoneNo) ? "" : "  Invalid Phone No";
            temp.phoneNo += fieldValues.phoneNo.length == 11 ? "" : " Please enter valid phone number";
        }
        if ('watch' in fieldValues) {
            temp.watch = "";
            temp.watch += fieldValues.watch ? "" : "This field is Required"
        }
        if ('close' in fieldValues) {
            temp.close = "";
            temp.close += fieldValues.close ? "" : "This field is Required"
        }
        if ('status' in fieldValues) {
            temp.status = "";
            temp.status += fieldValues.status ? "" : "This field is Required"
        }
        if ('accountType' in fieldValues) {
            temp.accountType = "";
            temp.accountType += fieldValues.accountType ? "" : "This field is Required"
        }
        //if ('sender' in fieldValues) {
        //    temp.sender = "";
        //    var pattern = new RegExp(/(^(\+88|0088)?(01){1}[3456789]{1}(\d){8})$/);
        //    temp.sender += fieldValues.sender ? "" : "This field is Required "
        //    temp.sender += pattern.test(fieldValues.sender) ? "" : "  Invalid Phone No";
        //    temp.sender += fieldValues.sender.length == 13 ? "" : " Please enter valid phone number";
        //}

        setErrors({
            ...temp
        })

        if (fieldValues == contactModel)
            return Object.values(temp).every(x => x == "")

    }



    const saveCustomer = (e) => {
        console.log(contactModel)
        e.preventDefault();

        if (validate()) {

            axios.post("api/contact/postContact", contactModel).then(res => {
                if (res.data != null) {

                    addToast("Submitted successfully", {
                        appearance: "success"
                    });
                    resetForm();
                }

                else {
                    addToast("Submitted successfully", {
                        appearance: "error"
                    });
                }


            })

        }

    }

    useEffect(() => {
        const abortController = new AbortController();
        axios.get("api/ussd/getAvailablePort", {
            signal: abortController.signal

        }).then(res => { setPortList(res.data) })

        return () => {
            abortController.abort();
        };
    },[])

   

    const handleInputChange = (e) => {
       
        const { name, value } = e.target;
        const fieldsValue = { [name]: value };
        
        setContactModel({ ...contactModel, ...fieldsValue });
       
        validate(fieldsValue)
    }
    return (<>
        <h1>Contact</h1>
        <form >
            <div className="form-group">
                <label >Name:</label>
                <input className="form-control" type="text" name="name" value={contactModel.name} onChange={handleInputChange} style={{ border: errors.name ? "1px solid red" : "1px solid #ced4da" }}/>
                {errors.name && <div style={{ color: "red" }}>{errors.name}</div>}
            </div>
            <div className="form-group">
                <label >Email:</label>
                <input className="form-control" type="text" name="email" value={contactModel.email} onChange={handleInputChange} style={{ border: errors.email ? "1px solid red" : "1px solid #ced4da" }}/>
                {errors.email && <div style={{ color: "red" }}>{errors.email}</div>}
            </div>
            <div className="form-group">
                <label >Synonym:</label>
                <input className="form-control" type="text" name="synonym" value={contactModel.synonym} onChange={handleInputChange} style={{ border: errors.name ? "1px solid red" : "1px solid #ced4da" }} />
                {errors.synonym && <div style={{ color: "red" }}>{errors.synonym}</div>}
            </div>
            <div className="form-group">
                <label >Phone No:</label>
                <input className="form-control" type="text" name="phoneNo" value={contactModel.phoneNo} onChange={handleInputChange} style={{ border: errors.phoneNo ? "1px solid red" : "1px solid #ced4da" }} />
                {errors.phoneNo && <div style={{ color: "red" }}>{errors.phoneNo}</div>}
            </div>
            <div className="form-group">
                <label >Watch Code:</label>
                <input className="form-control" type="text" name="watch" value={contactModel.watch} onChange={handleInputChange} style={{ border: errors.watch ? "1px solid red" : "1px solid #ced4da" }} />
                {errors.watch && <div style={{ color: "red" }}>{errors.watch}</div>}
            </div>
            <div className="form-group">
                <label >Close Code:</label>
                <input className="form-control" type="text" name="close" value={contactModel.close} onChange={handleInputChange} style={{ border: errors.close ? "1px solid red" : "1px solid #ced4da" }}/>
                {errors.close && <div style={{ color: "red" }}>{errors.close}</div>}
            </div>
            <div className="form-group">
                <label >Status Code:</label>
                <input className="form-control" type="text" name="status" value={contactModel.status} onChange={handleInputChange} style={{ border: errors.status ? "1px solid red" : "1px solid #ced4da" }}/>
                {errors.status && <div style={{ color: "red" }}>{errors.status}</div>}
            </div>
            <div className="form-group">
                <label >Sender:</label>
                <input className="form-control" type="text" name="sender" value={contactModel.sender} onChange={handleInputChange} style={{ border: errors.phoneNo ? "1px solid red" : "1px solid #ced4da" }} />
                {/*{errors.sender && <div style={{ color: "red" }}>{errors.sender}</div>}*/}
            </div>

           

            <button className="btn btn-danger" onClick={saveCustomer}>Save</button>
        </form>

    </>

    )
}

export default ContactAdd;