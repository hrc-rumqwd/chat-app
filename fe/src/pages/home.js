import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

function Home() {

    // const [currentPage, setCurrentPage] = useState(1);
    // useEffect(() => {
    //     ConversationService.GetConversations(currentPage, 10)
    //     .then(data => {
    //         console.log(data);
    //     });
    // }, []);

    return (
        <>
            <section className="h-100">
                <div className="container-fluid p-3 h-100">
                    <div className="row h-100 g-3">
                        <div className="col-md-4 col-lg-4 h-100 d-flex flex-column">
                        <h5 className="font-weight-bold mb-3">Member 
                            <span>
                                <FontAwesomeIcon icon={faPlus} className='fs-6' />
                            </span>
                            </h5>

                            <div className="card flex-grow-1 overflow-hidden">
                                <div className="card-body overflow-y-auto">
                                    <ul id="userList" className="list-unstyled mb-0">
                                    </ul>
                                    <div id="loadingTrigger" style={{ height: '20px' }}>
                                        <div className="skeleton-container" id="skeleton-loader" style={{ display: 'none' }}>
                                        <div className="skeleton-item">
                                            <div className="skeleton-avatar"></div>
                                            <div className="skeleton-text"></div>
                                        </div>
                                        <div className="skeleton-item">
                                            <div className="skeleton-avatar"></div>
                                            <div className="skeleton-text"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                        <div className="col-md-8 col-lg-8 h-100 d-flex flex-column">
                            <ul id="messageBoard" className="list-unstyled overflow-y-auto flex-grow-1 px-3 border rounded bg-light mb-3">
                            <div id="topSentinel" style={{ height: '2px' }}></div>
                            <div id="topSkeleton" style={{ display: 'none' }}></div>
                            </ul>
                            <div className="bg-white p-2 border-top">
                            <div data-mdb-input-init className="form-outline mb-2">
                                <textarea className="form-control bg-body-tertiary" id="messageInput" rows="4" placeholder="Message..."></textarea>
                            </div> 
                            <button type="button" data-mdb-button-init data-mdb-ripple-init className="btn btn-info btn-rounded float-end" onclick="sendMessage(event)">Send</button>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        <div id="conversationModalContainer"></div>
        </>
    );
}

export default Home;