import React from 'react';
import PropTypes from 'prop-types';
import { Controller } from "react-hook-form";
import Select from "@material-ui/core/Select";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";


SelectField.propTypes = {
    form: PropTypes.object.isRequired,
    name: PropTypes.string.isRequired,
    label: PropTypes.string,
    disabled: PropTypes.bool,
};

function SelectField(props) {
    const { form, name, labelId, label, disabled, required, children } = props
    const { errors, formState } = form
    const hasError = formState.touched[name] && errors[name]
    return (
        
        <Controller
            as={
                <Select labelId={labelId} label={label} outlined>
                    {children}
                </Select>
            }
            name={name}
            control={form.control}
            fullWidth
            disabled={disabled}
            variant="outlined"


            error={!!hasError}
            helperText={errors[name]?.message}
        />

    );
}

export default SelectField;