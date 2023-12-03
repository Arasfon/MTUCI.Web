/// <reference path="details.d.ts" />

import Editor from "@toast-ui/editor";
import Viewer from "@toast-ui/editor/dist/toastui-editor-viewer";

//import pluginColor from "@toast-ui/editor-plugin-color-syntax";
import pluginMergeCells from "@toast-ui/editor-plugin-table-merged-cell";
import pluginChart from "@toast-ui/editor-plugin-chart";
//import pluginCodeSyntaxHighlight from "@toast-ui/editor-plugin-code-syntax-highlight";

async function getArticle(articleIdString: string)
{
    const articleResponse = await fetch(`/api/news?id=${articleIdString}`);

    if (articleResponse.status === 200)
    {
        return (await articleResponse.json() as ArticleResponse).news;
    }
    else
    {
        return null;
    }
}

const pathArray = window.location.pathname.split("/");
const articleId = pathArray[pathArray.length - 1];

const urlParams = new URLSearchParams(window.location.search);
const isEditing = urlParams.get("edit") === "true";

const article = await getArticle(articleId);

if (article?.content === null)
{
    window.location.replace("/error/404");
}
else
{
    //document.getElementById("news-title")!.innerText = article!.title;

    if (isEditing)
    {
        const editor = new Editor({
            el: document.querySelector("#news-editor"),
            initialValue: article?.content,
            height: '80vh',
            previewStyle: 'vertical',
            theme: 'dark',
            plugins: [/*pluginColor,*/ pluginMergeCells, pluginChart, /*pluginCodeSyntaxHighlight*/]
        });

        document.getElementById("save-button")!.addEventListener("click", async e => {
            const response = await fetch(`/api/news?id=${articleId}`, {
                method: "put",
                body: editor.getMarkdown(),
                credentials: "include",
                headers: {
                    "Content-Type": "text/plain"
                }
            });

            if (response.status === 200)
            {
                window.location.href = window.location.pathname;
            }
            else
            {
                alert(response.status);
            }
        });
    }
    else
    {
        const viewer = new Viewer({
            el: document.querySelector("#news-viewer"),
            initialValue: article?.content,
            theme: 'dark',
            plugins: [/*pluginColor,*/ pluginMergeCells, pluginChart, /*pluginCodeSyntaxHighlight*/]
        });
    }
}
