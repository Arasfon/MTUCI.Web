import Editor from "@toast-ui/editor";

import pluginColor from "@toast-ui/editor-plugin-color-syntax";
import pluginMergeCells from "@toast-ui/editor-plugin-table-merged-cell";
import pluginChart from "@toast-ui/editor-plugin-chart";
import pluginCodeSyntaxHighlight from "@toast-ui/editor-plugin-code-syntax-highlight";

const editor = new Editor({
    el: document.querySelector("#news-editor"),
    initialValue: "Новая новость",
    height: '80vh',
    previewStyle: 'vertical',
    theme: 'dark',
    plugins: [pluginColor, pluginMergeCells, pluginChart, pluginCodeSyntaxHighlight]
});

document.getElementById("save-button")!.addEventListener("click", async e => {
    const response = await fetch("/api/news", {
        method: "post",
        body: editor.getMarkdown(),
        credentials: "include",
        headers: {
            "Content-Type": "text/plain"
        }
    });

    if (response.status === 201)
    {
        window.location.href = response.headers.get("location")!;
    }
    else
    {
        alert(response.status);
    }
});