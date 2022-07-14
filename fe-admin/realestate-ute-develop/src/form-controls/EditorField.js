import React, { useRef } from 'react';
import PropTypes from 'prop-types';
import { Controller } from "react-hook-form";
import { Editor } from "@tinymce/tinymce-react";


EditorField.propTypes = {
    form: PropTypes.object.isRequired,
    name: PropTypes.string.isRequired,
    label: PropTypes.string,
    disabled: PropTypes.bool,
};

function EditorField(props) {
    const { form, name, label, disabled, type, required, onEditorChange } = props
    const { errors, formState } = form
    const hasError = formState.touched[name] && errors[name]
    const unvalid = useRef(true)
    console.log(errors, unvalid)

    
    const handleEditorChange = (content, editor) => {
        console.log("Content was updated:", content);
        if (content === "")
            unvalid.current = true
        else
            unvalid.current = false
    };
    return (
        <Controller
            name={name}
            control={form.control}
            render={(
                { onChange, onBlur, value, name, ref },
                { invalid, isTouched, isDirty }
            ) => (
                <>
                    <Editor
                        initialValue="<p>This is the initial content of the editor</p>"
                        init={{
                            height: 500,
                            menubar: false,
                            plugins: [
                                "advlist autolink lists link image code charmap print preview anchor",
                                "searchreplace visualblocks code fullscreen",
                                "insertdatetime media table paste code help wordcount",
                            ],
                            toolbar:
                                "undo redo | formatselect | code |link | image | bold italic backcolor | alignleft aligncenter alignright alignjustify |  \n" +
                                "bullist numlist outdent indent | removeformat | help ",
                            content_style: 'body { color: #7e7e7e }'
                        }}
                        onEditorChange={handleEditorChange}
                    />
                    {hasError && (
                        <span>{errors[name]?.message}</span>
                   )}
                </>
            )}
        />


    );
}

export default EditorField;