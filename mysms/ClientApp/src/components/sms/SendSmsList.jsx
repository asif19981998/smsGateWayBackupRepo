import * as React from 'react';
import { useState, useRef } from 'react';
import PropTypes from 'prop-types';
import { alpha } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TablePagination from '@mui/material/TablePagination';
import TableRow from '@mui/material/TableRow';
import TableSortLabel from '@mui/material/TableSortLabel';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';
import Checkbox from '@mui/material/Checkbox';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import FormControlLabel from '@mui/material/FormControlLabel';
import Switch from '@mui/material/Switch';
import DeleteIcon from '@mui/icons-material/Delete';
import FilterListIcon from '@mui/icons-material/FilterList';
import { visuallyHidden } from '@mui/utils';
import { useEffect } from 'react';
import axios from 'axios';
import ReactHTMLTableToExcel from 'react-html-table-to-excel';
import XLSX from 'xlsx';


function createData(name, calories, fat, carbs, protein) {
    return {
        name,
        calories,
        fat,
        carbs,
        protein,
    };
}

function descendingComparator(a, b, orderBy) {
    if (b[orderBy] < a[orderBy]) {
        return -1;
    }
    if (b[orderBy] > a[orderBy]) {
        return 1;
    }
    return 0;
}

function getComparator(order, orderBy) {
    return order === 'desc'
        ? (a, b) => descendingComparator(a, b, orderBy)
        : (a, b) => -descendingComparator(a, b, orderBy);
}

function stableSort(array, comparator) {
    const stabilizedThis = array.map((el, index) => [el, index]);
    stabilizedThis.sort((a, b) => {
        const order = comparator(a[0], b[0]);
        if (order !== 0) {
            return order;
        }
        return a[1] - b[1];
    });
    return stabilizedThis.map((el) => el[0]);
}

const headCells = [
    {
        id: 'port',
        numeric: false,
        disablePadding: true,
        label: 'Port',
    },
    {
        id: 'phoneNo',
        numeric: true,
        disablePadding: false,
        label: 'Phone No',
    },
    {
        id: 'content',
        numeric: true,
        disablePadding: false,
        label: 'Message',
    },
    {
        id: 'sending Time',
        numeric: true,
        disablePadding: false,
        label: 'Sending Time',
    },
    {
        id: 'statusid',
        numeric: true,
        disablePadding: false,
        label: 'Status',
    },
];

function EnhancedTableHead(props) {

    const { onSelectAllClick, order, orderBy, numSelected, rowCount, onRequestSort } =
        props;
    const createSortHandler = (property) => (event) => {
        onRequestSort(event, property);
    };

    

    return (
        <TableHead>
            <TableRow>
                <TableCell padding="checkbox">
                    <Checkbox
                        color="primary"
                        indeterminate={numSelected > 0 && numSelected < rowCount}
                        checked={rowCount > 0 && numSelected === rowCount}
                        onChange={onSelectAllClick}
                        inputProps={{
                            'aria-label': 'select all desserts',
                        }}
                    />
                </TableCell>
                {headCells.map((headCell) => (
                    <TableCell
                        key={headCell.id}
                        align={headCell.numeric ? 'right' : 'left'}
                        padding={headCell.disablePadding ? 'none' : 'normal'}
                        sortDirection={orderBy === headCell.id ? order : false}
                    >
                        <TableSortLabel
                            active={orderBy === headCell.id}
                            direction={orderBy === headCell.id ? order : 'asc'}
                            onClick={createSortHandler(headCell.id)}
                        >
                            {headCell.label}
                            {orderBy === headCell.id ? (
                                <Box component="span" sx={visuallyHidden}>
                                    {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
                                </Box>
                            ) : null}
                        </TableSortLabel>
                    </TableCell>
                ))}
            </TableRow>
        </TableHead>
    );
}

EnhancedTableHead.propTypes = {
    numSelected: PropTypes.number.isRequired,
    onRequestSort: PropTypes.func.isRequired,
    onSelectAllClick: PropTypes.func.isRequired,
    order: PropTypes.oneOf(['asc', 'desc']).isRequired,
    orderBy: PropTypes.string.isRequired,
    rowCount: PropTypes.number.isRequired,
};

const EnhancedTableToolbar = (props) => {
    const { numSelected, handleInputChange, handleSearch, downloadExcel } = props;

    return (
        <>
            <h1 style={{ paddingLeft: "13px" }}>Send SMS </h1>
           <hr/>
        <div className="row" style={{ padding: "1%" }}>

            <div className="col-md-3 col-3">
                <div className="row">
                    <div className="col-md-6 col-6">
                        <div className="form-group">
                            <label>Synonym:</label>
                            <input name="synonym" className="form-control" placeholder="Synonym...." onChange={handleInputChange} />
                        </div>

                    </div>
                    <div className="col-md-6 col-6">
                        <div className="form-group">
                            <label>Phone No:</label>
                            <input name="sender" className="form-control" placeholder="PhoneNo...." onChange={handleInputChange} />
                        </div>

                    </div>

                </div>
            </div>
            <div className="col-md-3 col-3">
                <div className="row">
                    <div className="col-md-4 col-4">
                        <div className="form-group">
                            <label>Content:</label>
                            <input name="content" className="form-control" placeholder="Search...." onChange={handleInputChange} />
                        </div>

                    </div>
                    <div className="col-md-4 col-4">
                        <div className="form-group">
                            <label htmlFor="port">Port</label>
                            <select name="port" id="port" className="form-control" onChange={handleInputChange}>
                                <option value="null">---select a port---</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>

                            </select>
                        </div>

                        </div>
                        <div className="col-md-4 col-4">
                            <div className="form-group">
                                <label htmlFor="port">Status Code</label>
                                <select name="statusCode" id="statusCode" className="form-control" onChange={handleInputChange}>
                                    <option value="-1">---select a port---</option>
                                    <option value="0">Failed</option>
                                    <option value="1">Received</option>
                                    <option value="2">Pending</option>
                                    

                                </select>
                            </div>

                        </div>
                </div>
            </div>
            <div className="col-md-4 col-4">
                <div className="row">
                    <div className="col-md-6 col-6">
                        <div className="form-group">
                            <label for="startDate">Start Date:</label>
                            <input type="date" id="startDate" name="startDate" onChange={handleInputChange} className="form-control"/>
                        </div>
                    </div>
                    <div className="col-md-6 col-6">
                        <div className="form-group">
                            <label for="endDate">End Date:</label>
                            <input type="date" id="endDate" name="endDate" onChange={handleInputChange} className="form-control"/>
                        </div>
                    </div>

                </div>
            </div>

            <div className="col-md-2 col-2" style={{ paddingTop: "27px" }}>

                <div className="row" >
                    <div className="col-md-6 col-6">
                        <div className="form-group">
                            <button className="btn btn-success" onClick={handleSearch}>Search</button>
                        </div>
                    </div>
                    <div className="col-md-6 col-6">
                        <div className="form-group">
                            <button className="btn btn-info" onClick={downloadExcel}>Excel</button>
                        </div>
                    </div>

                </div>
            </div>




        </div>
        </>

    );
};

EnhancedTableToolbar.propTypes = {
    numSelected: PropTypes.number.isRequired,
};

export default function SendSmsItems() {
    const [rows, setRows] = useState([]);
    const [searchData, setSearchData] = useState({ synonym: "", sender: "", content: "", port: 0, startDate: "", endDtate: "",statusCode:0 });
    const url = "api/sms/getSendItem";


    useEffect(() => {

        axios.get(url).then(result => {
            setRows(result.data);
        })

       

    }, [])



    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setSearchData({ ...searchData, ...{ [name]: value } });
    }

    const handleSearch = (e) => {

        e.preventDefault();
        axios.post("api/sms/searchForSendingSms", searchData).then(res => {
            console.log(res.data);
                setRows(res.data);


        });
  }

    const [order, setOrder] = React.useState('asc');
    const [orderBy, setOrderBy] = React.useState('calories');
    const [selected, setSelected] = React.useState([]);
    const [page, setPage] = React.useState(0);
    const [dense, setDense] = React.useState(false);
    const [rowsPerPage, setRowsPerPage] = React.useState(500);

    const handleRequestSort = (event, property) => {
        const isAsc = orderBy === property && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        setOrderBy(property);
    };

    const handleSelectAllClick = (event) => {
        if (event.target.checked) {
            const newSelecteds = rows.map((n) => n.name);
            setSelected(newSelecteds);
            return;
        }
        setSelected([]);
    };

    const handleClick = (event, name) => {
        const selectedIndex = selected.indexOf(name);
        let newSelected = [];

        if (selectedIndex === -1) {
            newSelected = newSelected.concat(selected, name);
        } else if (selectedIndex === 0) {
            newSelected = newSelected.concat(selected.slice(1));
        } else if (selectedIndex === selected.length - 1) {
            newSelected = newSelected.concat(selected.slice(0, -1));
        } else if (selectedIndex > 0) {
            newSelected = newSelected.concat(
                selected.slice(0, selectedIndex),
                selected.slice(selectedIndex + 1),
            );
        }

        setSelected(newSelected);
    };

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const handleChangeDense = (event) => {
        setDense(event.target.checked);
    };

    const downloadExcel = () => {
        const newData = rows.map(row => {
            delete row.tableData
            return row
        })
        const workSheet = XLSX.utils.json_to_sheet(newData)
        const workBook = XLSX.utils.book_new()
        XLSX.utils.book_append_sheet(workBook, workSheet, "sendsms")
        //Buffer
        let buf = XLSX.write(workBook, { bookType: "xlsx", type: "buffer" })
        //Binary string
        XLSX.write(workBook, { bookType: "xlsx", type: "binary" })
        //Download
        XLSX.writeFile(workBook, "sendsmslist.xlsx")


    }

    const isSelected = (name) => selected.indexOf(name) !== -1;

    // Avoid a layout jump when reaching the last page with empty rows.
    const emptyRows =
        page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rows.length) : 0;

    return (
        <Box sx={{ width: '100%' }} >
          

            <Paper sx={{ width: '100%', mb: 2 }}>
                <EnhancedTableToolbar numSelected={selected.length} handleInputChange={handleInputChange} handleSearch={handleSearch} downloadExcel={downloadExcel} />
                <TableContainer>
                    <Table
                        id="table-to-xls"
                        sx={{ minWidth: 750 }}
                        aria-labelledby="tableTitle"
                        size={dense ? 'small' : 'medium'}

                    >
                        <EnhancedTableHead
                            numSelected={selected.length}
                            order={order}
                            orderBy={orderBy}
                            onSelectAllClick={handleSelectAllClick}
                            onRequestSort={handleRequestSort}
                            rowCount={rows.length}
                        />
                        <TableBody>
                           
                            {stableSort(rows, getComparator(order, orderBy))
                                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                                .map((row, index) => {
                                    const isItemSelected = isSelected(row.name);
                                    const labelId = `enhanced-table-checkbox-${index}`;

                                    return (
                                        <TableRow
                                            hover
                                           
                                            role="checkbox"
                                            aria-checked={isItemSelected}
                                            tabIndex={-1}
                                            key={row.id}
                                            selected={isItemSelected}
                                        >
                                            <TableCell padding="checkbox">
                                                <Checkbox
                                                    color="primary"
                                                    checked={isItemSelected}
                                                    inputProps={{
                                                        'aria-labelledby': labelId,
                                                    }}
                                                />
                                            </TableCell>
                                            <TableCell
                                                component="th"
                                                id={labelId}
                                                scope="row"
                                                padding="none"
                                            >
                                                {row.port}
                                            </TableCell>
                                            <TableCell align="right">{row.phoneNo}</TableCell>
                                            <TableCell align="right">{row.message}</TableCell>
                                            <TableCell align="right">{new Date(row.sendingTime).getDate()}-{new Date(row.sendingTime).toLocaleString("en-US", { month: "long" })}-{new Date(row.sendingTime).getFullYear()}</TableCell>
                                            {row.statusId == 2 ? <TableCell align="right" style={{ color: "orange" }}>Pending...</TableCell> : (row.statusId == 1 ? <TableCell align="right" style={{ color: "green" }}>Received</TableCell> : (row.statusId == 0 ? <TableCell align="right" style={{ color: "red" }}>Failed</TableCell> : "None"))}
                                     </TableRow>
                                    );
                                })}
                            {emptyRows > 0 && (
                                <TableRow
                                    style={{
                                        height: (dense ? 33 : 53) * emptyRows,
                                    }}
                                >
                                    <TableCell colSpan={6} />
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
                <TablePagination

                    rowsPerPageOptions={[500,1000,3000]}
                    component="div"
                    count={rows.length}
                    rowsPerPage={rowsPerPage}
                    page={page}
                    onPageChange={handleChangePage}
                    onRowsPerPageChange={handleChangeRowsPerPage}
                />
            </Paper>
            <FormControlLabel
                control={<Switch checked={dense} onChange={handleChangeDense} />}
                label="Dense padding"
            />
        </Box>
    );
}
