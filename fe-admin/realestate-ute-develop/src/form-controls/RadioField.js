import React from 'react';
import PropTypes from 'prop-types';
import { useForm, Controller } from "react-hook-form";
import {
    FormControl,
    FormControlLabel,
    FormLabel,
    Radio,
    RadioGroup,
} from "@material-ui/core";

RadioField.propTypes = {
    form: PropTypes.object.isRequired,
    name: PropTypes.string.isRequired,
    label: PropTypes.string,
    disabled: PropTypes.bool,
};



function RadioField(props) {
    const { form, name, disabled, required } = props
    
    
    return (
        <Controller
            as={
                <RadioGroup aria-label="gender" row>
                    <FormControlLabel
                        value="female"
                        control={<Radio />}
                        label="Nữ"
                    />
                    <FormControlLabel
                        value="male"
                        control={<Radio />}
                        label="Nam"
                    />
                </RadioGroup>
            }
            name={name}
            control={form.control}
            disabled={disabled}
            required={required}
        />
       
        
    );
}

export default RadioField;