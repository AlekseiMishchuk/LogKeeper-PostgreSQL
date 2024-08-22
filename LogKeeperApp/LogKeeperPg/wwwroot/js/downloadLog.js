function downloadLog(title, author, project, time, content) {
    const logContent = `Title: ${title}\nAuthor: ${author}\nProject: ${project}\nTime: ${time}\n\n${content}`;
    const blob = new Blob([logContent], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `${title}.txt`;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
}