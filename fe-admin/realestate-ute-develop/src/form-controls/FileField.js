import React from 'react';
import PropTypes from 'prop-types';
import { useForm, Controller } from "react-hook-form";
import { TextField } from '@material-ui/core';


FileField.propTypes = {
    form: PropTypes.object.isRequired,
    name: PropTypes.string.isRequired,
    label: PropTypes.string,
    disabled: PropTypes.bool,
};

function FileField(props) {
    const { form, name, label, disabled, handleFileChange } = props
    return (
        <Controller
            name={name}
            control={form.control}
            render={(
                { onChange, onBlur, value, name, ref },
                { invalid, isTouched, isDirty }
            ) => (
                <TextField
                    type="file"
                    fullWidth
                    label={label}
                    variant="outlined"
                    disabled={disabled}
                    onChange={(e)=>handleFileChange(e.target.files)}
                    value={value}
                    inputRef={ref} // wire up the input ref
                />
            )}

            // type="file"
            // fullWidth
            // label={label}
            // variant="outlined"
            // disabled={disabled}
        />


    );
}

export default FileField;