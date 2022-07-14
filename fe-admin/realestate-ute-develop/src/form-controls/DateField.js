import React from 'react';
import PropTypes from 'prop-types';
import { useForm, Controller } from "react-hook-form";
import { TextField } from '@material-ui/core';


DateField.propTypes = {
    form: PropTypes.object.isRequired,
    name: PropTypes.string.isRequired,
    label: PropTypes.string,
    disabled: PropTypes.bool,
};

function DateField(props) {
    const { form, name, label, disabled} = props
    return (
        <Controller
            name={name}
            control={form.control}
            as={TextField}

            fullWidth
            type="date"
            defaultValue="2017-05-24"
            variant="outlined"
            disabled={disabled}
        />



    );
}

export default DateField;