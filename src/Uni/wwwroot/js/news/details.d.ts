interface ArticleResponse
{
    news: News;
}

interface News
{
    id: number,
    title: string,
    date: string,
    authorId: number,
    content: string,
    author: null
}