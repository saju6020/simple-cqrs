let es: EventSource | null = null;

export function connect() {
    const receivedArea = document.getElementById("received_text_area") as HTMLElement;
    es = new EventSource(`http://localhost:5056/sse/connect`);
    es.onopen = (ev) => {
        console.log(ev);
    };
    es.onmessage = ev => {
        const newText = document.createElement("div");
        newText.textContent = ev.data;
        receivedArea.appendChild(newText);
    };
    es.onerror = ev => {
        console.error(ev);
    };
}
export function close() {
    es?.close();
}