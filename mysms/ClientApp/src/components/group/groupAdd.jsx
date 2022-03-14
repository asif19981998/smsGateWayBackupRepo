import React, { Component, useEffect, useState } from 'react';
import axios from "axios";
import { useToasts } from "react-toast-notifications";
const GroupAdd = (props) => {
    const { addToast } = useToasts();
    const [group, setGroup] = useState({ id: 0, name:"" });
    const [initialState, setInitialState] = useState({ id: 0, name: "" });
    const [errors, setErrors] = useState({})
  

    const resetForm = () => {
        setGroup(initialState);
    }


    const validate = (fieldValues = group) => {
        let temp = { ...errors }
        if ('name' in fieldValues) {
            temp.name = "";
            temp.name += fieldValues.name ? "" : "Name field is Required"
        }
        
        
        setErrors({
            ...temp
        })

        if (fieldValues == group)
            return Object.values(temp).every(x => x == "")

    }



    const saveGroup = (e) => {
        
        e.preventDefault();

        if (validate()) {

            axios.post("api/group", group).then(res => {
                
                if (res.data.isSuccess) {

                    addToast("Submitted successfully", {
                        appearance: "success"
                    });
                    resetForm();
                    props.updateTableData(res.data.result)
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
        
    }, [])



    const handleInputChange = (e) => {

        const { name, value } = e.target;
        const fieldsValue = { [name]: value };

        setGroup({ ...group, ...fieldsValue });

        validate(fieldsValue)
    }
    return (<>
        <h1>Group</h1>
        <form >
            <div className="form-group">
                <label >Name:</label>
                <input className="form-control" type="text" name="name" value={group.name} onChange={handleInputChange} style={{ border: errors.name ? "1px solid red" : "1px solid #ced4da" }} />
                {errors.name && <div style={{ color: "red" }}>{errors.name}</div>}
            </div>
         
            <button className="btn btn-danger" onClick={saveGroup}>Save</button>
        </form>

    </>

    )
}

export default GroupAdd;